using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    [SerializeField] private Tower _arrowTowerPrefab;
    [SerializeField] private Tower _fireTowerPrefab;
    [SerializeField] private Tower _iceTowerPrefab;

    [SerializeField] private Bullet _arrowBulletPrefab;
    [SerializeField] private Bullet _fireBulletPrefab;
    [SerializeField] private Bullet _iceBulletPrefab;

    [SerializeField] private Monster _normalmonsterPrefab1;
    [SerializeField] private Monster _normalmonsterPrefab2;
    [SerializeField] private Monster _elitemonsterPrefab1;
    [SerializeField] private Monster _elitemonsterPrefab2;
    [SerializeField] private Monster _bossmonsterPrefab1;
    [SerializeField] private Monster _bossmonsterPrefab2;

    public ObjectPool<Tower> _arrowTowerPool;
    public ObjectPool<Tower> _fireTowerPool;
    public ObjectPool<Tower> _iceTowerPool;

    public ObjectPool<Bullet> _arrowBulletPool;
    public ObjectPool<Bullet> _fireBulletPool;
    public ObjectPool<Bullet> _iceBulletPool;

    public ObjectPool<Monster> _monsterPool;
    public ObjectPool<Monster> _monsterPool2;
    public ObjectPool<Monster> _monsterPool3;
    public ObjectPool<Monster> _monsterPool4;
    public ObjectPool<Monster> _monsterPool5;
    public ObjectPool<Monster> _monsterPool6;
 

    private Dictionary<Gold, ObjectPool<Gold>> _goldPools
        = new Dictionary<Gold, ObjectPool<Gold>>();


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        
    }

    private void Start()
    {
        _arrowTowerPool = new ObjectPool<Tower>(
            _arrowTowerPrefab, 5, transform);

        _fireTowerPool = new ObjectPool<Tower>(
            _fireTowerPrefab, 5, transform);

        _iceTowerPool = new ObjectPool<Tower>(
            _iceTowerPrefab, 5, transform);

        _arrowBulletPool = new ObjectPool<Bullet>(
            _arrowBulletPrefab, 10, transform);

        _fireBulletPool = new ObjectPool<Bullet>(
            _fireBulletPrefab, 10, transform);

        _iceBulletPool = new ObjectPool<Bullet>(
            _iceBulletPrefab, 10, transform);

        // 기존 오브젝트 풀 생성
        _monsterPool = new ObjectPool<Monster>(
            _normalmonsterPrefab1,
             5,
            transform
        );
        
        _monsterPool2 = new ObjectPool<Monster>(
            _normalmonsterPrefab2,
            5,
            transform
        );
        _monsterPool3 = new ObjectPool<Monster>(
            _elitemonsterPrefab1,
            5,
            transform
        );
        _monsterPool4 = new ObjectPool<Monster>(
            _elitemonsterPrefab2,
            5,
            transform
        );
        _monsterPool5 = new ObjectPool<Monster>(
            _bossmonsterPrefab1,
            5,
            transform
        );
        _monsterPool6 = new ObjectPool<Monster>(
            _bossmonsterPrefab2,
            5,
            transform
        );
    }
    

    public Gold GetGold(
        Gold prefab,
        Vector3 spawnPosition,
        Quaternion spawnRotation,
        float lifetime)
    {
        if (!_goldPools.TryGetValue(prefab, out ObjectPool<Gold> pool))
        {
            pool = new ObjectPool<Gold>(
                prefab,
                0,
                transform
            );

            _goldPools.Add(prefab, pool);
        }


        Gold gold = pool.GetObject();

        gold.SetObjectPool(
            pool,
            lifetime
        );

        gold.transform.SetPositionAndRotation(
            spawnPosition,
            spawnRotation
        );

        pool.ActivateObject(gold);

        return gold;
    }
}