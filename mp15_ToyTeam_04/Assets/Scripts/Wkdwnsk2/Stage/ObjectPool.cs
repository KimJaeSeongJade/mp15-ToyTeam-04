using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component, IPoolable
{
    private T _monsterPrefab;
    private Transform _parent;

    private Queue<T> _monsterQueue = new Queue<T>();

    public ObjectPool(
        T monsterPrefab,
        int poolSize,
        Transform parent)
    {
        _monsterPrefab = monsterPrefab;
        _parent = parent;

        for (int i = 0; i < poolSize; i++)
        {
            CreateMonster();
        }
    }

    private void CreateMonster()
    {
        T monster = Object.Instantiate(_monsterPrefab, _parent);

        monster.gameObject.SetActive(false);
        _monsterQueue.Enqueue(monster);
    }

    public T GetMonster()
    {
        if (_monsterQueue.Count == 0)
        {
            CreateMonster();
        }

        return _monsterQueue.Dequeue();
    }
    
    public void ActivateMonster(T monster)
    {
        monster.gameObject.SetActive(true);
        monster.OnSpawn();
    }

    public void ReturnMonster(T monster)
    {
        if (monster.gameObject.activeSelf == false)
        {
            return;
        }

        // 비활성화하기 전에 정리
        monster.OnDespawn();

        monster.gameObject.SetActive(false);
        monster.transform.SetParent(_parent);

        _monsterQueue.Enqueue(monster);
    }
}