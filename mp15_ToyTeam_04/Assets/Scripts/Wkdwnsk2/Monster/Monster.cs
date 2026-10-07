using System;
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.AI;

public enum EMonsterType
{
    Normal1,
    Normal2,
    Elite1,
    Elite2,
    Boss1,
    Boss2
}

public class Monster : MonoBehaviour, IPoolable, IDamageable
{
    public EMonsterType EmonsterType;
    [SerializeField] private GoldDrop _goldDrop;


    private const string MONSTER_NAME = "일반 몬스터";
    private const int MONSTER_HEALTH = 10;
    private const float MONSTER_SPEED = 2f;
    private const int DROP_GOLD = 10;

    public string _monsterName = MONSTER_NAME;
    public int _monsterHealth = MONSTER_HEALTH;
    public float _monsterSpeed = MONSTER_SPEED;
    public int _dropGold = DROP_GOLD;
    
    //플레이어 스킬 
    private Coroutine _timeStopCoroutine;
   
    
    
    //목적지 도착
    public Transform endPoint;

    private Animator anim;
    
    // 오브젝트 풀 추가
    private ObjectPool<Monster> _objectPool;
    private NavMeshAgent _navmesh;

    private int currentHealth;
    public bool IsDead => _isDead;
    private bool _isDead;


    private bool hasUsedSurvival; // 보스 무적 스킬 썼는지
    private bool isSurvivalActive;  // 생존 스킬 활성화 중인지
    private bool _isBossImmune; // 보스 상태이상 면역 
    

    // 스테이지 증가량이 더해진 현재 값
    public float _bossRange = 1f;   // 기본 공격 범위
    private float _currentRange;
    private int _currentDropGold;
    
    // 타워 공격 효과 받을때 쓰는거
    private int _maxHealth; // 현재 최대 체력 (화상 % 계산용)
    private float _currentSpeed; // 둔화가 없을 때의 이동 속도
    private Coroutine _burnCoroutine;
    private Coroutine _slowCoroutine;
    
    // 몬스터 UI 체력 위치 전달용
    [SerializeField] private HP _hpPrefab;      // HP UI 프리팹
    [SerializeField] private float _hpHeight = 2f;  // HP바 위치
    private HP _hp;                             // 몬스터 HP UI                             
    

    private void Awake()
    {
        _navmesh = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }
    
    

    //몬스터 스폰
    public void OnSpawn()
    {
        // 몬스터 타워 추가 효과 받을 때 
        _maxHealth = _monsterHealth;
        _currentSpeed = _monsterSpeed;
        _burnCoroutine = null;
        _slowCoroutine = null;
        
        
        // 체력과 전투 상태 초기화
        _currentRange = _bossRange;
        _currentDropGold = _dropGold;
        currentHealth = _monsterHealth;
        _isDead = false;
        isSurvivalActive = false;
        _timeStopCoroutine = null;

        // 사망 애니메이션 초기화
        anim.Rebind();
        anim.Update(0f);

        // 사망할 때 껐던 기능 복구
        GetComponent<Collider>().enabled = true;

        _navmesh.enabled = true;
        _navmesh.speed = _monsterSpeed;
        
        // HP UI 만들고 켜기
        CreateHpUI();

        if (_hp != null)
        {
            _hp.gameObject.SetActive(true);
        }

        if (endPoint == null)
        {
            Debug.LogWarning("몬스터의 목적지가 없음.");
            return;
        }

        if (_navmesh.isOnNavMesh == false)
        {
            Debug.LogWarning("몬스터가 NavMesh 위에 없음.");
            return;
        }

        _navmesh.isStopped = false;
        _navmesh.SetDestination(endPoint.position);
    }
    
    public void OnDespawn()
    {
        // 반납 전에 진행 중이던 작업 정리
        StopAllCoroutines();
        _timeStopCoroutine = null;
        _burnCoroutine = null;
        _slowCoroutine = null;
        
        // HP UI 끄기
        if (_hp != null)
        {
            _hp.gameObject.SetActive(false);
        }
        
        if (_navmesh.enabled && _navmesh.isOnNavMesh)
        {
            _navmesh.ResetPath();
        }

        _navmesh.enabled = false;
        GetComponent<Collider>().enabled = false;

        isSurvivalActive = false;
        endPoint = null;
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(false,100);

            Debug.Log(_monsterName + " 현재 체력 : " + currentHealth);
        }
        
        // 테스트 : 화상 (최대 체력 50%, 2초)
        if (Input.GetKeyDown(KeyCode.F))
        {
            BulletAffect burnAffect = new BulletAffect(EBulletAffectType.Burn, true, 50, 2);
            BulletAffect(burnAffect);

            Debug.Log(_monsterName + " 화상 적용");
        }
        
        // 테스트 : 감속 (이동 속도 70% 감소, 1초)
        if (Input.GetKeyDown(KeyCode.P))
        {
            BulletAffect slowAffect = new BulletAffect(EBulletAffectType.DecreaseSpeed, true, 70, 1);
            BulletAffect(slowAffect);

            Debug.Log(_monsterName + " 이동속도 : " + _navmesh.speed);
        }
        
    }
    


    public void TakeDamage(bool per, int damage)
    {
        if (_isDead || isSurvivalActive)
        {
            return;
        }
        // 엘리트2가 천재지변이 아니라면 1 데미지
        if (EmonsterType == EMonsterType.Elite2)
        {
            MinimumDamage();
            return;
        }

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
        

        // HP UI 갱신
        UpdateHpUI(damage);

        if (currentHealth <= 0)
        {
            if (EmonsterType == EMonsterType.Boss1 && !hasUsedSurvival)
            {
                StartCoroutine(BossResurrect());
            }

            else
            {        
                MonsterDead();
            }
        } 
    }

    private IEnumerator BossResurrect()
    {
        hasUsedSurvival = true;
        isSurvivalActive = true;
        currentHealth = 1;

        Debug.Log(_monsterName + "보스 스킬 발동");
        yield return new WaitForSeconds(5f);

        currentHealth += 1;
        isSurvivalActive = false;

    }

    private bool IsBossImmune()
    {
        if( EmonsterType == EMonsterType.Boss2)
        {
            return true;
        }
        return false;
    }



    private void MinimumDamage()
    {
        currentHealth -= 1;
        // HP UI 갱신
        UpdateHpUI(1);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            MonsterDead();
        }
    }
    
    

    private void MonsterDead()
    {
        if (_isDead)
        {
            return;
        }
        
        
        _navmesh.ResetPath(); // 사망시 이동하지 않게 수정
        _navmesh.enabled = false; // 사망시 길찾는 navmesh 기능 끄기
        GetComponent<Collider>().enabled = false; // 사망시 다른 몬스터가 멈칫하지 않도록 콜리더 끄기


        _isDead = true;
        Debug.Log(_monsterName + " 사망");

        anim.SetBool("IsDead", true);
        StartCoroutine(DeadWait());
        
        // 플레이어 골드 증가
        if (GoldManager.Instance != null)
        {
            GoldManager.Instance.AddGold(_currentDropGold);}
        
    }

    private IEnumerator DeadWait()
    {
        yield return new WaitForSeconds(2f);
        _objectPool.ReturnObject(this);
        
    }
    
    // 포탈에 닿았을 때 삭제처리 (애니메이션 안나옴)
    public void MonsterDelete()
    {
        if (_navmesh.isOnNavMesh)
        {
            _navmesh.ResetPath();
        }

        
        _objectPool.ReturnObject(this);
        
    }
    
    public void SetObjectPool(ObjectPool<Monster> objectPool)
    {
        _objectPool = objectPool;
    }
    
    
    //스킬 구현
    public void ApplyTimeFreeze(float duration)
    {
        if (_isDead)
        {
            return;
        }

        // Boss2는 상태이상 면역
        if (IsBossImmune())
        {
            return;
        }

        // 이미 동결 중이라면 기존 시간을 취소하고
        // 새로 3초를 시작
        if (_timeStopCoroutine != null)
        {
            StopCoroutine(_timeStopCoroutine);
        }

        _timeStopCoroutine = StartCoroutine(
            TimeStopRoutine(duration)
        );
    }
    
    private IEnumerator TimeStopRoutine(float duration)
    {

        if (_navmesh.enabled &&
            _navmesh.isOnNavMesh)
        {
            _navmesh.isStopped = true;
        }

        yield return new WaitForSeconds(duration);


        if (_navmesh.enabled &&
            _navmesh.isOnNavMesh &&
            !_isDead)
        {
            _navmesh.isStopped = false;
        }

        _timeStopCoroutine = null;
    }
    
    // 전체 공격 수정 필요
    public void SkillDamage(int percent)
    {
        if (_isDead)
        {
            return;
        }

        int damage = Mathf.CeilToInt(
            _monsterHealth * percent
        );

        damage = Mathf.Max(damage, 1);
        
        TakeDamage(false, damage);
    }

    // 몬스터 생성
    public static Monster GetMonster(
        EMonsterType monsterType,
        Vector3 spawnPosition,
        Quaternion spawnRotation,
        Transform endPoint)
    {
        ObjectPool<Monster> pool = null;


        // 몬스터 종류에 맞는 풀 선택
        if (monsterType == EMonsterType.Normal1)
        { 
            pool = PoolManager.Instance._monsterPool;
        }
        else if (monsterType == EMonsterType.Normal2)
        {
            pool = PoolManager.Instance._monsterPool2;
        }
        else if (monsterType == EMonsterType.Elite1)
        {
            pool = PoolManager.Instance._monsterPool3;
        }
        else if (monsterType == EMonsterType.Elite2)
        {
            pool = PoolManager.Instance._monsterPool4;
        }
        else if (monsterType == EMonsterType.Boss1)
        {
            pool = PoolManager.Instance._monsterPool5;
        }
        else if (monsterType == EMonsterType.Boss2)
        {
            pool = PoolManager.Instance._monsterPool6;
        }


        // 풀에서 몬스터 가져오기
        Monster monster = pool.GetObject();


        // 자신이 어느 풀에서 나왔는지 저장
        monster.SetObjectPool(pool);


        GoldDrop goldDrop = monster.GetComponentInChildren<GoldDrop>(true);

        if (goldDrop != null)
        {
            goldDrop.SetPoolManager(
                PoolManager.Instance
            );
        }


        // 위치 설정
        monster.transform.SetPositionAndRotation(
            spawnPosition,
            spawnRotation
        );


        // 목적지 설정
        monster.endPoint = endPoint;
        
        // 몬스터 활성화
        pool.ActivateObject(monster);
        
        return monster;
    }


    public void ReturnMonster(Monster monster)
    {
        _objectPool.ReturnObject(monster);
    }

    public static Gold GetGold(
        ObjectPool<Gold> pool,
        Vector3 spawnPosition,
        Quaternion spawnRotation,
        float lifetime)
    {
        Gold gold = pool.GetObject();

        gold.SetObjectPool(pool, lifetime);

        gold.transform.SetPositionAndRotation(
            spawnPosition,
            spawnRotation
        );

        // 골드활성화
        pool.ActivateObject(gold);

        return gold;
    }
    
    // Stage에서 스폰 직후 호출 : 기본 능력치 + 스테이지 증가량
    public void SetStageStat(int plusHealth, float plusSpeed, float plusRange, int plusGold)
    {
        _maxHealth = _monsterHealth + plusHealth; //화상용 최대 체력
        currentHealth = _maxHealth;
        _currentSpeed = _monsterSpeed + plusSpeed;
        _navmesh.speed = _currentSpeed;
        _currentRange = _bossRange + plusRange;
        _currentDropGold = _dropGold + plusGold;
        
        // 시작 체력 표시
        UpdateHpUI(currentHealth);
    }
    
       // 타워 공격 효과 적용 (IDamageable)
    public void BulletAffect(BulletAffect affect)
    {
        if (affect == null || _isDead)
        {
            return;
        }

        // Boss2는 상태이상 면역
        if (IsBossImmune())
        {
            return;
        }

        // 타워가 보낸 효과 종류에 따라 적용
        if (affect.EBulletAffectType == EBulletAffectType.Burn)
        {
            // 이미 화상 중이면 처음부터 다시
            if (_burnCoroutine != null)
            {
                StopCoroutine(_burnCoroutine);
            }

            _burnCoroutine = StartCoroutine(BurnRoutine(affect));
        }
        else if (affect.EBulletAffectType == EBulletAffectType.DecreaseSpeed)
        {
            // 이미 둔화 중이면 처음부터 다시
            if (_slowCoroutine != null)
            {
                StopCoroutine(_slowCoroutine);
            }

            _slowCoroutine = StartCoroutine(SlowRoutine(affect));
        }
    }
    
    // 화상 : 타워의 Duration초 동안 1초마다 데미지
    private IEnumerator BurnRoutine(BulletAffect affect)
    {
        int damage;

        if (affect.IsPer)
        {
            damage = _maxHealth * affect.AffectValue / 100;
        }
        else
        {
            
            damage = affect.AffectValue;
        }

        
        float time = 0f;

        while (time < affect.Duration)
        {
            yield return new WaitForSeconds(1f);
            time = time + 1f;

            if (_isDead)
            {
                break;
            }

            TakeDamage(false, damage);
        }

        _burnCoroutine = null;
    }

    // 몬스터 이속 감소
    private IEnumerator SlowRoutine(BulletAffect affect)
    {
        float slowSpeed;


        // AffectValue 만큼 감소
        slowSpeed = _currentSpeed * (100 - affect.AffectValue) / 100f;    

        if (slowSpeed < 0f)
        {
            slowSpeed = 0f;
        }

        _navmesh.speed = slowSpeed;

        yield return new WaitForSeconds(affect.Duration);

        // 원래 속도로 복구
        if (_isDead == false)
        {
            _navmesh.speed = _currentSpeed;
        }

        _slowCoroutine = null;
    }
    
    // HP UI 만들기 (몬스터마다 처음 한 번만)
    private void CreateHpUI()
    {
        // 이미 만들었으면 다시 안 만듦
        if (_hp != null)
        {
            return;
        }

        if (_hpPrefab == null)
        {
            return;
        }

    }

    // HP UI에 체력과 데미지 보내기
    private void UpdateHpUI(int damage)
    {
        if (_hp == null)
        {
            return;
        }

        _hp.MonsterStateUpdate(currentHealth, _maxHealth, damage);
    }

    
    
    

}