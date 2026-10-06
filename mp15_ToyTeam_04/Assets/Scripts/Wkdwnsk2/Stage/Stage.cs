using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Stage : MonoBehaviour

{
    [HideInInspector] public int StageNumber;
    [HideInInspector] public int WaveNumber;
    [HideInInspector] public  int MonstersNumber;
    [HideInInspector] public int StageClearReward;
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
    
    private const int WAVE_COUNT = 5;
    private int TotalMonsterNumber;
    //팝업 누르면 웨이브 시작
    private bool _waitNextWave = false;
    [SerializeField] private int _stageClearGold =500;

 
    
    
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
        if (Input.GetKeyDown(KeyCode.M))
        {
            NextWave();
        }
        
        
    }
    
 


    private IEnumerator StageStart()
    {
        _isStageRunning = true;
        
        // 스테이지 상태 초기화
        _currentStageLife = _maxStageLife;
        _isStageFailed = false;
        
        
        Debug.Log(StageNumber + " 스테이지 시작");
        
        for (int wave = 1; wave <= WAVE_COUNT; wave++)
        {
            WaveNumber = wave;
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
            
            if (wave == WAVE_COUNT)
            {
                StageClear();
            }

    
            {
                // NextWave()가 호출될 때까지 대기 (여기서 팝업 띄우기)
                _waitNextWave = true;

                while (_waitNextWave)
                {
                    yield return null;
                }

                Debug.Log("다음 웨이브까지 " + _waveInterval + "초");

                yield return new WaitForSeconds(_waveInterval);
            }
        }


    }
    
    // 팝업 일때는 어떻게 연동 할지
    public void NextWave()
    {
        _waitNextWave = false;
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
        
        StartCoroutine(StageLoop());
        
    }
    
    private IEnumerator StageLoop()
    {
        _isStageRunning = true;
        _isStageFailed = false;
        StageNumber = 1;

        while (_isStageFailed == false)
        {
            // 스테이지 하나(5웨이브) 진행
            yield return StartCoroutine(StageStart());

            if (_isStageFailed)
            {
                break;
            }
            
 

            // 다음 스테이지로
            StageNumber = StageNumber + 1;

            Debug.Log("다음 스테이지까지 " + _waveInterval + "초");

            yield return new WaitForSeconds(_waveInterval);
        }

        _isStageRunning = false;
    }
    
    


    private IEnumerator SpawnWave()
    {
        // 이전 웨이브 몬스터 목록 초기화
        _spawnedMonsters.Clear();
        TotalMonsterNumber = 0;
        MonstersNumber = 0;

        int normalCount = GetNormalCount();
        int eliteCount = GetEliteCount();
        int bossCount = GetBossCount();
        
        TotalMonsterNumber = normalCount + eliteCount + bossCount;
        
        EMonsterType normalType;
        EMonsterType eliteType;
        EMonsterType bossType;

        if (StageNumber % 2 == 1)
        {
            normalType = EMonsterType.Normal1;
            eliteType = EMonsterType.Elite1;
            bossType = EMonsterType.Boss1;
        }
        
        else 
        {
            normalType = EMonsterType.Normal2;
            eliteType = EMonsterType.Elite2;
            bossType = EMonsterType.Boss2;
        }

        // 노말 몬스터 생성
        for (int i = 0; i < normalCount; i++)
        {
            if (_isStageFailed)
            {
                yield break;
            }
            
            CreateMonster(normalType);
            yield return new WaitForSeconds(_spawnInterval);
        }
        
        // 엘몬 생성
        for (int i = 0; i < eliteCount; i++)
        {
            if (_isStageFailed)
            {
                yield break;
            }
            
            CreateMonster(eliteType);
            yield return new WaitForSeconds(_spawnInterval);

        }
        
        // 보스몬스터 생성
        for (int i = 0; i < bossCount; i++)
        {
            if (_isStageFailed)
            {
                yield break;
            }
            
            CreateMonster(bossType);
            yield return new WaitForSeconds(_spawnInterval);

        }
       
    }

    private int GetNormalCount()
    {
        return (StageNumber * 3) + (WaveNumber / 3) + (WaveNumber);
    }

    private int GetEliteCount()
    {
        if (WaveNumber < 3)
        {
            return 0;
        }
        return (WaveNumber / 2);
    }

    private int GetBossCount()
    {
        if (WaveNumber < 5)
        {
            return 0;
        }

        return (StageNumber / 10) + (WaveNumber / 5);
    }


    private void CreateMonster(EMonsterType monsterType)
    {
        Monster monster = Monster.GetMonster(
            monsterType,
            _curMap.SpawnPoint.position,
            _curMap.SpawnPoint.rotation,
            _curMap.ArrivalPoint
        );

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
        
        // 진행 중이던 스테이지/웨이브 코루틴 모두 정지
        StopAllCoroutines();
        

        Debug.Log("스테이지 패배");
        // 스테이지 패배 시 UI 추가 필요
        
    }
    
    
    // 스테이지 클리어 보상 : 500 x 스테이지 번호
    private void StageClear()
    {
        StageClearReward = (_stageClearGold * StageNumber);
            
        Debug.Log(StageNumber + " 스테이지 클리어");
        Debug.Log("클리어 보상 : " + StageClearReward);

        // 플레이어 골드 증가
        if (GoldManager.Instance != null)
        {
            GoldManager.Instance.AddGold(StageClearReward);
        }
    }



    

    
    
}