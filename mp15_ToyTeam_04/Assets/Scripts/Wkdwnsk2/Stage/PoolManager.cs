using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private Monster _monsterPrefab;
    [SerializeField] private int _poolSize = 1;
    [SerializeField] private Map _curMap;

    private ObjectPool<Monster> _objectPool;
    private Dictionary<Gold, ObjectPool<Gold>> _goldPools  = new Dictionary<Gold, ObjectPool<Gold>>();

    private void Awake()
    {

        _objectPool = new ObjectPool<Monster>(
            _monsterPrefab,
            _poolSize,
            transform
        );
    }
    public bool IsReady()
    {
        return isActiveAndEnabled && _objectPool != null;
    }

    public Monster GetMonster(
        Vector3 spawnPosition,
        Quaternion spawnRotation,
        Transform endPoint)
    {
        Monster monster = _objectPool.GetObject();

        monster.SetObjectPool(this);

        GoldDrop goldDrop =
            monster.GetComponentInChildren<GoldDrop>(true);

        if (goldDrop != null)
        {
            goldDrop.SetPoolManager(this);
        }

        monster.transform.SetPositionAndRotation(
            spawnPosition,
            spawnRotation
        );

        monster.endPoint = endPoint;

        // 목적지 확인
        _objectPool.ActivateObject(monster);

        return monster;
    }
    

    public void ReturnMonster(Monster monster)
    {
        _objectPool.ReturnObject(monster);
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

        gold.SetObjectPool(pool, lifetime);

        gold.transform.SetPositionAndRotation(
            spawnPosition,
            spawnRotation
        );

        pool.ActivateObject(gold);

        return gold;
    }
    
    
}