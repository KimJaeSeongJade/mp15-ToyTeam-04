using System.Collections;
using System.Collections.Generic;
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
    public bool IsReady { get; private set; }

    private void Start()
    {
        if (Instance != this) return;
        StartCoroutine(MonsterPoolCreat());
    }

    private IEnumerator MonsterPoolCreat()
    {
        IsReady = false;
        _arrowTowerPool = new ObjectPool<Tower>(_arrowTowerPrefab, 0, transform);
        _fireTowerPool = new ObjectPool<Tower>(_fireTowerPrefab, 0, transform);
        _iceTowerPool = new ObjectPool<Tower>(_iceTowerPrefab, 0, transform);
        _arrowBulletPool = new ObjectPool<Bullet>(_arrowBulletPrefab, 0, transform);
        _fireBulletPool = new ObjectPool<Bullet>(_fireBulletPrefab, 0, transform);
        _iceBulletPool = new ObjectPool<Bullet>(_iceBulletPrefab, 0, transform);
        _monsterPool = new ObjectPool<Monster>(_normalmonsterPrefab1, 0, transform);
        _monsterPool2 = new ObjectPool<Monster>(_normalmonsterPrefab2, 0, transform);
        _monsterPool3 = new ObjectPool<Monster>(_elitemonsterPrefab1, 0, transform);
        _monsterPool4 = new ObjectPool<Monster>(_elitemonsterPrefab2, 0, transform);
        _monsterPool5 = new ObjectPool<Monster>(_bossmonsterPrefab1, 0, transform);
        _monsterPool6 = new ObjectPool<Monster>(_bossmonsterPrefab2, 0, transform);

        yield return null;

        yield return PrewarmPool(_arrowTowerPool, 5);
        yield return PrewarmPool(_fireTowerPool, 5);
        yield return PrewarmPool(_iceTowerPool, 5);
        yield return PrewarmPool(_arrowBulletPool, 5);
        yield return PrewarmPool(_fireBulletPool, 5);
        yield return PrewarmPool(_iceBulletPool, 5);
        yield return PrewarmPool(_monsterPool, 5);
        yield return PrewarmPool(_monsterPool2, 5);
        yield return PrewarmPool(_monsterPool3, 5);
        yield return PrewarmPool(_monsterPool4, 5);
        yield return PrewarmPool(_monsterPool5, 5);
        yield return PrewarmPool(_monsterPool6, 5);

        IsReady = true;
    }

    private IEnumerator PrewarmPool<T>(ObjectPool<T> pool, int count)
        where T : Component, IPoolable
    {
        for (int i = 0; i < count; i++)
        {
            pool.CreateObject();
            yield return null;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
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
