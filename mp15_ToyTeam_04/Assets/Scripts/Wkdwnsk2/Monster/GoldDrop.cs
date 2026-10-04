using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GoldDrop : MonoBehaviour
{
    [Header("코인 프리팹")]
    [SerializeField] private GameObject GoldPrefab;

    [Header("코인 위치")]
    [SerializeField] private Transform dropPoint;

    [Header("코인 지속 시간")]
    [SerializeField, Min(0f)] private float GoldLifetime = 2f;

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

        Gold Gold = GoldPoolManager.Instance.GetGold(
            prefab,
            spawnPosition,
            Quaternion.identity,
            GoldLifetime
        );

        if (Gold != null)
        {
            hasDropped = true;
        }
    }
}