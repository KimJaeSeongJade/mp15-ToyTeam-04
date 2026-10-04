using System.Collections;
using UnityEngine;

public class Gold : MonoBehaviour, IPoolable
{
    private ObjectPool<Gold> _objectPool;
    private float _GoldLifetime;

    public void SetObjectPool(
        ObjectPool<Gold> objectPool,
        float GoldLifetime)
    {
        _objectPool = objectPool;
        _GoldLifetime = Mathf.Max(0f, GoldLifetime);
    }

    public void OnSpawn()
    {
        StopAllCoroutines();
        StartCoroutine(ReturnAfterLifetime());
    }

    public void OnDespawn()
    {
        StopAllCoroutines();
    }

    private IEnumerator ReturnAfterLifetime()
    {
        yield return new WaitForSeconds(_GoldLifetime);

        _objectPool.ReturnObject(this);
    }
}