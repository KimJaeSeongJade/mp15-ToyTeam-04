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
    public float stoppingDistanceThreshold = 0.1f;

    public Animator anim;
    
    // 오브젝트 풀 추가
    private ObjectPool<Monster> _objectPool;
    private NavMeshAgent _navmesh;

    private int currentHealth;
    private bool isDead;

    private bool hasUsedSurvival; // 보스 무적 스킬 썼는지
    private bool isSurvivalActive;  // 생존 스킬 활성화 중인지
    private bool _isBossImmune; // 보스 상태이상 면역 
    

    private void Awake()
    {
        _navmesh = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }

    //몬스터 스폰
    public void OnSpawn()
    {
        // 체력과 전투 상태 초기화
        currentHealth = _monsterHealth;
        isDead = false;
        isSurvivalActive = false;
        _timeStopCoroutine = null;

        // 사망 애니메이션 초기화
        anim.Rebind();
        anim.Update(0f);

        // 사망할 때 껐던 기능 복구
        GetComponent<Collider>().enabled = true;

        _navmesh.enabled = true;
        _navmesh.speed = _monsterSpeed;

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
        
        
    }
    


    public void TakeDamage(bool per, int damage)
    {
        if (isDead || isSurvivalActive)
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

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            MonsterDead();
        }
    }
    
    

    private void MonsterDead()
    {
        if (isDead)
        {
            return;
        }
        
        _navmesh.ResetPath(); // 사망시 이동하지 않게 수정
        _navmesh.enabled = false; // 사망시 길찾는 navmesh 기능 끄기
        GetComponent<Collider>().enabled = false; // 사망시 다른 몬스터가 멈칫하지 않도록 콜리더 끄기


        isDead = true;
        Debug.Log(_monsterName + " 사망");

        anim.SetBool("IsDead", true);
        StartCoroutine(DeadWait());


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
        if (isDead)
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
            !isDead)
        {
            _navmesh.isStopped = false;
        }

        _timeStopCoroutine = null;
    }
    
    // 전체 공격 수정 필요
    public void SkillDamage(int percent)
    {
        if (isDead)
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

}