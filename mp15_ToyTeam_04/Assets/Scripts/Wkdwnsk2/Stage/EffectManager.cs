using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager> 
{
    [Header("이펙트 프리팹")]
    [SerializeField] private Effect[] _effectPrefabs;     

    [Header("이펙트 유지 시간")]
    [SerializeField] private float _effectLifeTime = 2f;
    private int _poolSize = 5;

    // 이펙트 종류별 풀
    private Dictionary<EEffectType, ObjectPool<Effect>> _effectPools;

    public enum EEffectType
    {
        MONSTER_HIT,
        TOWER_INSTALL,
        SKILL_USE
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        // 배열 순서 = enum 순서
        _effectPools = new Dictionary<EEffectType, ObjectPool<Effect>>();
        for (int i = 0; i < _effectPrefabs.Length; i++)
        {
            if (_effectPrefabs[i] == null) continue;

            _effectPools[(EEffectType)i] = new ObjectPool<Effect>(_effectPrefabs[i], _poolSize, transform);
        }
    }

    // 이펙트 사용시 EffectManager.Instance.PlayEffect(타입, 위치) 
    // 이펙트 생성 위치
    public void PlayEffect(EEffectType effectType, Vector3 position)
    {
        if (_effectPools.TryGetValue(effectType, out ObjectPool<Effect> pool) == false)
        {
            Debug.Log("이펙트 없음");
            return;
        }

        Effect effect = pool.GetObject();                  
        effect.SetObjectPool(pool, _effectLifeTime);       
        effect.transform.SetPositionAndRotation(position, Quaternion.identity); 
        pool.ActivateObject(effect);                      
    }
}