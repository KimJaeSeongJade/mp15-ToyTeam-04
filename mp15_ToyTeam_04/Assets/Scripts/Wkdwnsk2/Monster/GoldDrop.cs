using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldDrop : MonoBehaviour
{
    [Header("코인 프리팹")]
    [SerializeField] private GameObject coinPrefab;

    [Header("코인 위치")]
    [SerializeField] private Transform dropPoint;

    [Header("코인 지속 시간")]
    [SerializeField, Min(0f)] private float coinLifetime = 2f;

    private Animator animator;
    private bool hasDropped;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        hasDropped = false;
    }

    public void DropCoin()
    {
        if (!animator.GetBool("IsDead") || hasDropped)
        {
            return;
        }

        if (coinPrefab == null)
        {
            return;
        }

        hasDropped = true;

        Vector3 spawnPosition = dropPoint != null
            ? dropPoint.position
            : transform.position;

        // 코인 생성
        GameObject coin = Instantiate(
            coinPrefab,
            spawnPosition,
            Quaternion.identity
        );

        // 지속 시간 후 삭제
        Destroy(coin, coinLifetime);
    }
}

