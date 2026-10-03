using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    private Tower _tower;
    [SerializeField] private SphereCollider _col;
    [SerializeField] private LayerMask _targetLayerMask;
    private List<GameObject> _monsterList = new();

    private void Awake() => CacheComponents();

    private void Start()
    {
        _col.radius = _tower.TowerDetectionRange(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & _targetLayerMask.value) != 0)
        {
            if (!_monsterList.Contains(other.gameObject))
            {
                _monsterList.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & _targetLayerMask.value) != 0)
        {
            if (_monsterList.Contains(other.gameObject))
            {
                _monsterList.Remove(other.gameObject);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_tower == null)
        {
            CacheComponents();
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.up / 2, _tower.TowerDetectionRange(false));
    }

    private void CacheComponents()
    {
        _tower = GetComponent<Tower>();
    }
}