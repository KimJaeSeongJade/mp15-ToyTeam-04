using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MonsterWaveData
{
    public GameObject MonsterPrefab;
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
    
    private List<Monster> _spawnedMonsters = new List<Monster>();
    
    private void Awake()
    {
        _curMap = GetComponent<Map>();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (_isStageRunning == false)
            {
                StartCoroutine(StageStart());
            }
        }
    }

    private IEnumerator StageStart()
    {
        _isStageRunning = true;
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
            Debug.Log(WaveNumber + " 웨이브 몬스터 생성 완료"
            );
            
            // 모든 몬스터가 없어질 때까지 기다림
            yield return new WaitUntil(() => IsWaveClear());
            
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

        // 모든 웨이브 종료
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
        if (_isStageRunning == true)
        {
            return;
        }
        StartCoroutine(SpawnWave());
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
                CreateMonster(
                    monsterData.MonsterPrefab
                );
                
                yield return new WaitForSeconds(
                    _spawnInterval
                );
            }
        }
    }


    private void CreateMonster(
        GameObject monsterPrefab)
    {
        if (monsterPrefab == null)
        { 
            Debug.LogWarning("몬스터 프리팹이 연결되지 않았습니다.");
            return;
        }


        GameObject monsterObject =
            Instantiate(monsterPrefab, _curMap.SpawnPoint.position, _curMap.SpawnPoint.rotation);
        Monster monster =
            monsterObject.GetComponentInChildren<Monster>();


        if (monster == null)
        {
            Debug.LogWarning(monsterPrefab.name + "에 Monster 스크립트가 없습니다.");
            
            Destroy(monsterObject);

            return;
        }


        monster.endPoint =
            _curMap.ArrivalPoint;


        _spawnedMonsters.Add(
            monster
        );
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


    private bool IsStageClear()
    {
        return StageClearCondition();
    }
    
    private bool StageClearCondition()
    {
        if (WaveNumber == _waveData.Length
            && IsWaveClear() == true)
        { return true; }
        return false;
    }
}