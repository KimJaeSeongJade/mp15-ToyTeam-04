using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable] public class MonsterWaveData
{
    public PoolManager MonsterPool;
    public int MonstersNumber;
}

[System.Serializable]
public class WaveData
{
    public MonsterWaveData[] Monsters;
}

public class Stage : MonoBehaviour
{
    [SerializeField] private int StageNumber = 1;
    [SerializeField] private int WaveNumber = 1;
    [SerializeField] private int MonstersNumber;
    [SerializeField] private int StageClearReward;
    [SerializeField] private WaveData[] _waveData;
    [SerializeField] private float _spawnInterval = 1.0f;
    [SerializeField] private float _waveInterval = 3.0f;
    private Map _curMap;
    private bool _isStageRunning = false;
    
    //스테이지 라이프 구현
    [Header("스테이지 라이프")]
    [SerializeField] private int _maxStageLife = 10;
    [SerializeField] private int _monsterLifeDamage = 1;
    private int _currentStageLife;
    private bool _isStageFailed;
    
   
 
    
    
    private List<Monster> _spawnedMonsters = new List<Monster>();
    
    private void Awake()
    {
        _curMap = GetComponent<Map>();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            MonsterGenerate();
        }
        
    }
    
 


    private IEnumerator StageStart()
    {
        _isStageRunning = true;
        
        // 스테이지 상태 초기화
        _currentStageLife = _maxStageLife;
        _isStageFailed = false;
        
        
        Debug.Log(StageNumber + " 스테이지 시작");
        
        for (int i = 0; i < _waveData.Length; i++)
        {
            WaveNumber = i + 1;
            Debug.Log(
                "===== "
                + WaveNumber
                + " 웨이브 시작 ====="
            );
            
            // 현재 웨이브 몬스터 생성
            yield return StartCoroutine(
                SpawnWave()
            );
            
            if (_isStageFailed)
            {
                yield break;
            }
            
            Debug.Log(WaveNumber + " 웨이브 몬스터 생성 완료"
            );
            
            // 몬스터 다 잡을때까지 대기
            yield return new WaitUntil(
                () => IsWaveClear() || _isStageFailed
            );

            if (_isStageFailed)
            {
                yield break;
            }
            
            Debug.Log("===== " + WaveNumber + " 웨이브 클리어 =====");


            // 마지막 웨이브가 아니면 대기
            if (i < _waveData.Length - 1)
            {
                Debug.Log("다음 웨이브까지 " + _waveInterval + "초"
                );
                
                yield return new WaitForSeconds(
                    _waveInterval
                );
            }
        }

        // 웨이브 끝
        if (IsStageClear() == true)
        {
            StageClearReward = StageNumber * 500;
            
            Debug.Log(StageNumber + " 스테이지 클리어");
            Debug.Log("클리어 보상 : " + StageClearReward);
        }
        _isStageRunning = false;
    }


    public void MonsterGenerate()
    {
        // 웨이브 시작했는지 확인
        if (_isStageRunning)
        {
            return;
        }

        if (_curMap == null ||
            _curMap.SpawnPoint == null ||
            _curMap.ArrivalPoint == null)
        {
            Debug.LogError("Map과 출발점, 도착점을 확인해주세요.");
            return;
        }

        if (_waveData == null || _waveData.Length == 0)
        {
            Debug.LogError("웨이브를 설정해주세요.");
            return;
        }

        // 시작 전에 모든 웨이브 설정 확인
        for (int i = 0; i < _waveData.Length; i++)
        {
            WaveData wave = _waveData[i];

            if (wave == null ||
                wave.Monsters == null ||
                wave.Monsters.Length == 0)
            {
                Debug.LogError((i + 1) + "웨이브의 몬스터 목록이 없습니다.");
                return;
            }

            for (int j = 0; j < wave.Monsters.Length; j++)
            {
                MonsterWaveData monsterData = wave.Monsters[j];

                if (monsterData == null ||
                    monsterData.MonsterPool == null)
                {
                    Debug.LogError((i + 1) + "웨이브의 풀을 연결해주세요.");
                    return;
                }

                if (monsterData.MonsterPool.IsReady() == false)
                {
                    Debug.LogError((i + 1) + "웨이브의 풀이 준비되지 않았습니다.");
                    return;
                }

                if (monsterData.MonstersNumber <= 0)
                {
                    Debug.LogError("몬스터 수는 1 이상으로 설정해주세요.");
                    return;
                }
            }
        }

        StartCoroutine(StageStart());
    }
    


    private IEnumerator SpawnWave()
    {
        // 이전 웨이브 몬스터 목록 초기화
        _spawnedMonsters.Clear();
        MonstersNumber = 0;
        
        WaveData currentWave = _waveData[WaveNumber - 1];
        
        // 이번 웨이브 전체 몬스터 수 계산
        for (int i = 0; i < currentWave.Monsters.Length; i++)
        {
            MonstersNumber += currentWave.Monsters[i].MonstersNumber;
        }


        Debug.Log(WaveNumber + " 웨이브 몬스터 수 : " + MonstersNumber);


        // 설정된 순서대로 생성
        for (int i = 0;
             i < currentWave.Monsters.Length;
             i++)
        {
            MonsterWaveData monsterData =
                currentWave.Monsters[i];


            for (int j = 0;
                 j < monsterData.MonstersNumber;
                 j++)
            {
                if (_isStageFailed)
                {
                    yield break;
                }
                
                CreateMonster(
                    monsterData.MonsterPool
                );
                
                yield return new WaitForSeconds(
                    _spawnInterval
                );
            }
        }
    }


    private void CreateMonster(PoolManager monsterPool)
    {
        Monster monster = monsterPool.GetMonster(
            _curMap.SpawnPoint.position,
            _curMap.SpawnPoint.rotation,
            _curMap.ArrivalPoint
        );
        

        // 같은 객체가 웨이브 도중 재사용되면 중복 등록하지 않음
        if (_spawnedMonsters.Contains(monster) == false)
        {
            _spawnedMonsters.Add(monster);
        }
    }

    private bool IsWaveClear()
    {
        for (int i = _spawnedMonsters.Count - 1;
             i >= 0;
             i--)
        {
            Monster monster =
                _spawnedMonsters[i];


            // Destroy된 경우
            if (monster == null)
            {
                _spawnedMonsters.RemoveAt(i);

                continue;
            }


            // SetActive(false) 된 경우
            if (monster.gameObject.activeInHierarchy == false)
            {
                _spawnedMonsters.RemoveAt(i);
            }
        }


        if (_spawnedMonsters.Count == 0)
        {
            return true;
        }


        return false;
    }
    
    public void MonsterReachedGoal(Monster monster)
    {
        if (!_isStageRunning)
        {
            return;
        }

        if (_isStageFailed)
        {
            return;
        }

        // 현재 웨이브 몬스터 목록에서 제거
        if (_spawnedMonsters.Remove(monster) == false)
        {
            return;
        }

        // 스테이지 라이프 감소
        _currentStageLife -= _monsterLifeDamage;

        // 0 아래로 내려가지 않게
        _currentStageLife = Mathf.Max(
            _currentStageLife,
            0
        );

        Debug.Log(
            "스테이지 라이프 : "
            + _currentStageLife
        );

        // 라이프가 모두 떨어졌으면 패배
        if (_currentStageLife <= 0)
        {
            StageFail();
        }
    }
    
    // 스테이지 패배
    private void StageFail()
    {
        if (_isStageFailed)
        {
            return;
        }

        _isStageFailed = true;
        _isStageRunning = false;

        Debug.Log("스테이지 패배");
    }


    private bool IsStageClear()
    {
        return StageClearCondition();
    }
    
    private bool StageClearCondition()
    {
        if (_isStageFailed)
        {
            return false;
        }

        if (WaveNumber == _waveData.Length
            && IsWaveClear() == true)
        { return true; }
        return false;
    }
    
    
}