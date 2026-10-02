using System;
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

public class Monster : MonoBehaviour
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
    private NavMeshAgent agent;
    public float stoppingDistanceThreshold = 0.1f; 




    private NavMeshAgent _navmesh;

    private int currentHealth;

    private void Awake()
    {
        _navmesh = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        currentHealth = _monsterHealth;

        _navmesh.speed = _monsterSpeed;



        
    }

    private void Start()
    {
        if (endPoint != null)
        {
            _navmesh.SetDestination(endPoint.position);
        }
        
        agent = GetComponent<NavMeshAgent>();
        
        if (endPoint != null)
        {
            agent.SetDestination(endPoint.position);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(3);

            Debug.Log(_monsterName + " 현재 체력 : " + currentHealth);
        }
        
        if (IsTargetReached())
        {
            MonsterDelete();
        }
    }
    
    bool IsTargetReached()
    {
        // 1. 아직 경로를 계산 중인 경우(pathPending)에는 도착한 것이 아님
        if (agent.pathPending|| !agent.isActiveAndEnabled || !agent.isOnNavMesh) return false;

        // 2. 남은 거리가 에이전트의 정지 거리 + 오차 범위 이하인지 확인
        if (agent.remainingDistance <= agent.stoppingDistance + stoppingDistanceThreshold)
        {
            // 3. 경로가 없거나 속도가 거의 zero일 때 최종 도착으로 판정
            if (!agent.hasPath || agent.velocity.sqrMagnitude <= 0.2f)
            {
                return true;
            }
        }

        return false;
    }

    public void TakeDamage(int damage)
    {
        if (monsterType == MonsterType.Elite2)
        {
            MinimumDamage();
            return;
        }

        currentHealth -= damage;


        if (currentHealth <= 0)
        {
            if (monsterType == MonsterType.Boss1)
            {
                
            }

            else
            {
                MonsterDead();
            }
        } 
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
        Debug.Log(_monsterName + " 사망");

        gameObject.SetActive(false);
    }
    
    public void MonsterDelete()
    {
        CancelInvoke();

        if (_navmesh.isOnNavMesh)
        {
            _navmesh.ResetPath();
        }

        gameObject.SetActive(false);
    }
}