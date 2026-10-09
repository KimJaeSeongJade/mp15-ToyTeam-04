using System.Collections;
using UnityEngine;

public class Effect : MonoBehaviour, IPoolable
{
    private ObjectPool<Effect> _objectPool;
    private float _lifeTime;

    // 몇 초 뒤에 돌아가는지
    public void SetObjectPool(ObjectPool<Effect> objectPool, float lifeTime)
    {
        _objectPool = objectPool;
        _lifeTime = lifeTime;
    }

    public void OnSpawn()
    {
        StartCoroutine(EffectTime());
    }

    public void OnDespawn()
    {
        StopAllCoroutines();
    }

    // 시간이 지나면 풀로 반납
    private IEnumerator EffectTime()
    {
        yield return new WaitForSeconds(_lifeTime);
        _objectPool.ReturnObject(this);
    }
}