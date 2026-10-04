using System.Collections.Generic;
using UnityEngine;

public class GoldPoolManager : MonoBehaviour
{
    public static GoldPoolManager Instance { get; private set; }

    [Header("골드 프리팹")]
    [SerializeField] private Gold[] _GoldPrefabs;
    
    private readonly Dictionary<Gold, ObjectPool<Gold>> _GoldPools
        = new Dictionary<Gold, ObjectPool<Gold>>();

    private void Awake()
    {


        Instance = this;

        if (_GoldPrefabs == null)
        {
            return;
        }

        foreach (Gold prefab in _GoldPrefabs)
        {
            
            if (_GoldPools.ContainsKey(prefab))
            {
                continue;
            }

            _GoldPools.Add(
                prefab,
                new ObjectPool<Gold>(
                    prefab,
                    0,
                    transform
                )
            );
        }
    }

    public Gold GetGold(
        Gold prefab,
        Vector3 spawnPosition,
        Quaternion spawnRotation,
        float GoldLifetime)
    {
        if (prefab == null ||
            !_GoldPools.TryGetValue(prefab, out ObjectPool<Gold> pool))
        {
            Debug.LogError("골드 없음", this);
            return null;
        }

        Gold Gold = pool.GetObject();

        Gold.SetObjectPool(pool, GoldLifetime);
        Gold.transform.SetPositionAndRotation(
            spawnPosition,
            spawnRotation
        );

        pool.ActivateObject(Gold);

        return Gold; 
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}