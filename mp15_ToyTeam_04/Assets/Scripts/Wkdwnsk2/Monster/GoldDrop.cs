using UnityEngine;

public class GoldDrop : MonoBehaviour
{
    [Header("골드 프리팹")]
    [SerializeField] private GameObject GoldPrefab;

    [Header("골드 위치")]
    [SerializeField] private Transform dropPoint;

    [Header("골드 지속 시간")]
    [SerializeField, Min(0f)] private float GoldLifetime = 2f;

    private Animator animator;
    private bool hasDropped;
    private PoolManager _poolManager;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        hasDropped = false;
    }

    public void SetPoolManager(PoolManager poolManager)
    {
        _poolManager = poolManager;
    }

    public void DropGold()
    {
        if (animator == null ||
            !animator.GetBool("IsDead") ||
            hasDropped)
        {
            return;
        }
        

        Gold prefab = GoldPrefab.GetComponent<Gold>();

        Vector3 spawnPosition = dropPoint != null
            ? dropPoint.position
            : transform.position;

        Gold gold = _poolManager.GetGold(
            prefab,
            spawnPosition,
            Quaternion.identity,
            GoldLifetime
        );

        if (gold != null)
        {
            hasDropped = true;
        }
    }
}