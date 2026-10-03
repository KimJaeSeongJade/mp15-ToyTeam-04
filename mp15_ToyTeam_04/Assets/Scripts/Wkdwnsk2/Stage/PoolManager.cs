using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private Monster _monsterPrefab;
    [SerializeField] private int _poolSize = 1;
    [SerializeField] private Map _curMap;

    private ObjectPool<Monster> _objectPool;

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
        Monster monster = _objectPool.GetMonster();

        monster.SetObjectPool(this);

        monster.transform.position = spawnPosition;
        monster.transform.rotation = spawnRotation;
        monster.endPoint = endPoint;

        // 목적지 확인
        _objectPool.ActivateMonster(monster);
        
        return monster;
    }

    public void ReturnMonster(Monster monster)
    {
        _objectPool.ReturnMonster(monster);
    }
}