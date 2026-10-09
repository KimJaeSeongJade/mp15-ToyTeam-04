using System.Collections;
using UnityEngine;

public class Effect : MonoBehaviour, IPoolable
{
    [Header("이펙트 지속 시간")]
    [SerializeField] private float _lifeTime = 2f;   

    private ObjectPool<Effect> _objectPool;

    // 돌아갈 풀만 받기
    public void SetObjectPool(ObjectPool<Effect> objectPool)
    {
        _objectPool = objectPool;
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