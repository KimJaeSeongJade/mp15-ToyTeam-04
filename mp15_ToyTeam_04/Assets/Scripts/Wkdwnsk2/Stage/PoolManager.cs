using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    [SerializeField] private Tower _towerPrefab;

    [SerializeField] private Monster _normalmonsterPrefab1;
    [SerializeField] private Monster _normalmonsterPrefab2;
    [SerializeField] private Monster _elitemonsterPrefab1;
    [SerializeField] private Monster _elitemonsterPrefab2;
    [SerializeField] private Monster _bossmonsterPrefab1;
    [SerializeField] private Monster _bossmonsterPrefab2;
    [SerializeField] private Map _curMap;

    public ObjectPool<Tower> _towerPool;

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
        _towerPool = new ObjectPool<Tower>(
            _towerPrefab, 26, transform);

        // 기존 오브젝트 풀 생성
        _monsterPool = new ObjectPool<Monster>(
            _normalmonsterPrefab1,
             20,
            transform
        );
        
        _monsterPool2 = new ObjectPool<Monster>(
            _normalmonsterPrefab2,
            20,
            transform
        );
        _monsterPool3 = new ObjectPool<Monster>(
            _elitemonsterPrefab1,
            20,
            transform
        );
        _monsterPool4 = new ObjectPool<Monster>(
            _elitemonsterPrefab2,
            20,
            transform
        );
        _monsterPool5 = new ObjectPool<Monster>(
            _bossmonsterPrefab1,
            20,
            transform
        );
        _monsterPool6 = new ObjectPool<Monster>(
            _bossmonsterPrefab2,
            20,
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