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
        return Monster.GetMonster(
            _objectPool,
            this,
            spawnPosition,
            spawnRotation,
            endPoint
        );
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
        // 프리팹별 풀 생성과 보관은 PoolManager가 담당
        if (!_goldPools.TryGetValue(prefab, out ObjectPool<Gold> pool))
        {
            pool = new ObjectPool<Gold>(prefab, 0, transform);
            _goldPools.Add(prefab, pool);
        }

        return Monster.GetGold(
            pool,
            spawnPosition,
            spawnRotation,
            lifetime
        );
    }
    
}