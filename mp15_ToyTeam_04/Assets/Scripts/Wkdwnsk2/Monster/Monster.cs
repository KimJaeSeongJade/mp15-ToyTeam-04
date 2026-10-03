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

    public Animator anim;
    private WaitForSeconds waitTime = new WaitForSeconds(3f);





    private NavMeshAgent _navmesh;

    private int currentHealth;

    private void Awake()
    {
        _navmesh = GetComponent<NavMeshAgent>();
        anim = this.GetComponent<Animator>();

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
        
        
    }
    
    void OnTriggerEnter(Collider other)
    {
        MonsterDelete();
        
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

        anim.SetTrigger("Dead");
        StartCoroutine(DeadWait());


    }

    private IEnumerator DeadWait()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);

    }
    
    public void MonsterDelete()
    {

        if (_navmesh.isOnNavMesh)
        {
            _navmesh.ResetPath();
        }

        gameObject.SetActive(false);
    }
}