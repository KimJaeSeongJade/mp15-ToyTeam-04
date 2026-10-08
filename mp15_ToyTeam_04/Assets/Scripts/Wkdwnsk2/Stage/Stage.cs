using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Stage : MonoBehaviour

{
    [HideInInspector] public int StageNumber;
    [HideInInspector] public int WaveNumber;
    [HideInInspector] public int totalMonsterNumber;
    [HideInInspector] public int monsterNumber;
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
    //팝업 누르면 웨이브 시작
    private bool _waitNextWave = false;
    [SerializeField] private int _stageClearGold =500;
    
    
    private List<Monster> _spawnedMonsters = new List<Monster>();


    public void Start()
    {
        StartCoroutine(AutoStart());
    }

  
    
    // 맵이 나오면 n초 기다렸다가 자동으로 시작
    private IEnumerator AutoStart()
    {
        // 로비에서 게임 시작을 눌러 맵이 나올 때까지 대기
        while (MapManager.Instance._curMap == null)
        {
            yield return null;
        }

        Debug.Log(_waveInterval + "초 후 몬스터 소환 시작");

        yield return new WaitForSeconds(_waveInterval);

        MonsterGenerate();
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
                UIManager.Instance.PopUp.StageResultPopUpOpen();
                StageClear();
            }


            {
                // 마지막 웨이브가 아니면 _waveInterval초 후 다음 웨이브 자동 시작
                if (wave < WAVE_COUNT)
                {
                    Debug.Log("다음 웨이브까지 " + _waveInterval + "초");

                    yield return new WaitForSeconds(_waveInterval);
                }
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

        if (MapManager.Instance._curMap == null ||
            MapManager.Instance._curMap.SpawnPoint == null ||
            MapManager.Instance._curMap.ArrivalPoint == null)
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
        totalMonsterNumber = 0;
        monsterNumber = 0;

        int normalCount = GetNormalCount();
        int eliteCount = GetEliteCount();
        int bossCount = GetBossCount();
        
        totalMonsterNumber = normalCount + eliteCount + bossCount;
        
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
            MapManager.Instance._curMap.SpawnPoint.position,
            MapManager.Instance._curMap.SpawnPoint.rotation,
            MapManager.Instance._curMap.ArrivalPoint
        );

        // 체력 : 보스는 PlusBossHealth, 나머지는 PlusStageHealth
        int plusHealth;

        if (monsterType == EMonsterType.Boss1 || monsterType == EMonsterType.Boss2)
        {
            plusHealth = PlusBossHealth();
        }
        else
        {
            plusHealth = PlusStageHealth(monsterType);
        }

        // 스테이지/웨이브에 맞게 능력치 적용
        monster.SetStageStat(
            plusHealth,
            PlusStageSpeed(monsterType),
            PlusStageRange(monsterType),
            PlusStageGold(monsterType)
            
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
    
    //웨이브 당 능력치 증가 함수
    [System.Serializable]
    public class MonsterStatUp
    {
        public int PlusWaveHealth;
        public int PlusStageHealth;
        public float PlusWaveSpeed;
        public float PlusStageSpeed;
        public float PlusWaveRange;
        public float PlusStageRange;
        public int PlusWaveGold;
        public int PlusStageGold;
    }
    
    [SerializeField] private MonsterStatUp _normalStatUp;
    [SerializeField] private MonsterStatUp _eliteStatUp;
    [SerializeField] private MonsterStatUp _bossStatUp;



    
    // 지금까지 클리어한 웨이브 수 (0부터 시작, 1-2면 1, 2-1이면 5)
    private int GetClearedWaveCount()
    { 
        return (StageNumber - 1) * WAVE_COUNT + (WaveNumber - 1);
    }

    // 지금까지 클리어한 스테이지 수
    private int GetClearedStageCount()
    {
        return StageNumber - 1;
    }

    // 몬스터 종류에 따른 스탯
    private MonsterStatUp GetStatUp(EMonsterType type)
    {
        if (type == EMonsterType.Normal1 || type == EMonsterType.Normal2)
        {
            return _normalStatUp;
        }

        if (type == EMonsterType.Elite1 || type == EMonsterType.Elite2)
        {
            return _eliteStatUp;
        }

        return _bossStatUp;
    }

    // 체력 계산 (일반 + 엘리트)
    public int PlusStageHealth(EMonsterType type)
    {
        MonsterStatUp statUp = GetStatUp(type);

        return (statUp.PlusWaveHealth * GetClearedWaveCount())
               + (statUp.PlusStageHealth * GetClearedStageCount());
    }

    // 보스 체력 계산
    public int PlusBossHealth()
    {
        return (_bossStatUp.PlusWaveHealth * GetClearedWaveCount())
               + (_bossStatUp.PlusStageHealth * GetClearedStageCount());
    }

    // 이동 속도 계산
    public float PlusStageSpeed(EMonsterType type)
    {
        MonsterStatUp statUp = GetStatUp(type);

        return (statUp.PlusWaveSpeed * GetClearedWaveCount())
               + (statUp.PlusStageSpeed * GetClearedStageCount());
    }

    // 공격 범위 계산
    public float PlusStageRange(EMonsterType type)
    {
        MonsterStatUp statUp = GetStatUp(type);

        return (statUp.PlusWaveRange * GetClearedWaveCount())
               + (statUp.PlusStageRange * GetClearedStageCount());
    }

    // 획득 골드 계산
    public int PlusStageGold(EMonsterType type)
    {
        MonsterStatUp statUp = GetStatUp(type);

        return (statUp.PlusWaveGold * GetClearedWaveCount())
               + (statUp.PlusStageGold * GetClearedStageCount());

    }
    


    

    
    
}