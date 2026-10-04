using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum MonsterType
{
    Normal1,
    Normal2,
    Elite1,
    Elite2,
    Boss1,
    Boss2
}

public class Monster : MonoBehaviour, IPoolable
{
    public MonsterType monsterType;

    private const string MONSTER_NAME = "일반 몬스터";
    private const int MONSTER_HEALTH = 10;
    private const float MONSTER_SPEED = 2f;
    private const int DROP_GOLD = 10;

    public string _monsterName = MONSTER_NAME;
    public int _monsterHealth = MONSTER_HEALTH;
    public float _monsterSpeed = MONSTER_SPEED;
    public int _dropGold = DROP_GOLD;
    
    
    //목적지 도착
    public Transform endPoint;
    public float stoppingDistanceThreshold = 0.1f;

    public Animator anim;
    
    // 오브젝트 풀 추가
    private PoolManager _objectPool;    

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
            TakeDamage(3);

            Debug.Log(_monsterName + " 현재 체력 : " + currentHealth);
        }
        
        
    }
    
    void OnTriggerEnter(Collider other)
    {
        MonsterDelete();
        
    }

    public void TakeDamage(int damage)
    {
        if (isDead || isSurvivalActive)
        {
            return;
        }
        if (monsterType == MonsterType.Elite2)
        {
            MinimumDamage();
            return;
        }

        currentHealth -= damage;


        if (currentHealth <= 0)
        {
            if (monsterType == MonsterType.Boss1 && !hasUsedSurvival)
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

        currentHealth = 1 + 1;
        isSurvivalActive = false;

    }

    private bool IsBossImmune()
    {
        if( monsterType == MonsterType.Boss2)
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
        if (_objectPool != null)
        {
            _objectPool.ReturnMonster(this);
        }
        else
        {
            gameObject.SetActive(false);
        }

    }
    
    public void MonsterDelete()
    {
        if (_navmesh.isOnNavMesh)
        {
            _navmesh.ResetPath();
        }


        if (_objectPool != null)
        {
            _objectPool.ReturnMonster(this);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    
    public void SetObjectPool(PoolManager objectPool)
    {
        _objectPool = objectPool;
    }
}