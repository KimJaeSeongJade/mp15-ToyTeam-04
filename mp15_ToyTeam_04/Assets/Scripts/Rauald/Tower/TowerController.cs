using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    [SerializeField] private Tower _tower;
    [SerializeField] private Transform _head;
    [SerializeField] private LayerMask _targetLayerMask;
    private List<GameObject> _monsterList = new();

    private float _curTime;
    [SerializeField] private Bullet _bullet;
    [SerializeField] private Transform[] _muzzles;

    private void Awake() => CacheComponents();

    private void OnEnable()
    {
        _curTime = 0f;
    }

    private void Update()
    {
        if (_monsterList.Count == 0) return;

        LookAtTarget();

        if (_tower.State == null) return;

        _curTime += Time.deltaTime;

        if(_curTime > _tower.State.AtkSpeed)
        {
            Instantiate(_bullet, _muzzles[_tower.State.CurLevel - 1].position, _muzzles[_tower.State.CurLevel - 1].rotation).Init(_tower.State.Affect, _monsterList[0].transform, _tower.TowerAtk(), _monsterList[0].GetComponent<IDamageable>());
            _curTime = 0;
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

    private void CacheComponents()
    {
        _tower = GetComponent<Tower>();
    }
}