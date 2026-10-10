using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component, IPoolable
{
    private T _Prefab;
    private Transform _parent;

    private Queue<T> _poolQueue = new Queue<T>();

    public ObjectPool(T Prefab, int poolSize, Transform parent)
    {
        _Prefab = Prefab;
        _parent = parent;
    }

    public void CreateObject()
    {
        T item = Object.Instantiate(_Prefab, _parent);

        item.gameObject.SetActive(false);
        _poolQueue.Enqueue(item);
    }

    public T GetObject()
    {
        if (_poolQueue.Count == 0)
        {
            CreateObject();
        }

        return _poolQueue.Dequeue();
    }
    
    public void ActivateObject(T item)
    {
        item.gameObject.SetActive(true);
        item.OnSpawn();
    }

    public void ReturnObject(T item)
    {
        if (item.gameObject.activeSelf == false)
        {
            return;
        }

        // 비활성화하기 전에 정리
        item.OnDespawn();

        item.gameObject.SetActive(false);
        item.transform.SetParent(_parent);

        _poolQueue.Enqueue(item);
    }
}