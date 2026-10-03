using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    [SerializeField] private Tower _tower;
    [SerializeField] private Transform _head;
    [SerializeField] private SphereCollider _col;
    [SerializeField] private LayerMask _targetLayerMask;
    private List<GameObject> _monsterList = new();

    [SerializeField] private Bullet _bullet;
    [SerializeField] private Transform[] _muzzles;

    private void Awake() => CacheComponents();

    private void Start()
    {
        _col.radius = _tower.TowerDetectionRange(false);
    }

    private void Update()
    {
        if (_monsterList.Count == 0) return;

        LookAtTarget();

        if (Input.GetKeyDown(KeyCode.A))
        {
            Instantiate(_bullet, _muzzles[_tower.CurLevel - 1].position, _muzzles[_tower.CurLevel - 1].rotation).Init(_tower.ETowerType, _monsterList[0].transform, _tower.TowerAtk(false), _monsterList[0].GetComponent<IDamageable>());
        }
    }

    private void LookAtTarget()
    {
        if (_head == null) return;

        Vector3 target = _monsterList[0].transform.position - _head.position;
        float angle = -Mathf.Atan2(target.z, target.x);
        _head.rotation = Quaternion.Euler(0f, 90f + angle * Mathf.Rad2Deg, 0f);
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
        _tower.TowerBasicInstall();
    }
}