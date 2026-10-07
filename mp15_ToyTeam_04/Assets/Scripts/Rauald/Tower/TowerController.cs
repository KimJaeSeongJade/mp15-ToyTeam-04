using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    [SerializeField] private Tower _tower;
    [SerializeField] private Transform _head;
    [SerializeField] private LayerMask _targetLayerMask;
    private List<Monster> _monsterList = new();

    private float _curTime;
    private ObjectPool<Bullet> _bulletPool;
    [SerializeField] private Bullet _bullet;
    [SerializeField] private Transform[] _muzzles;

    private void Awake() => CacheComponents();

    private void OnEnable()
    {
        _curTime = 0f;

        switch (_tower.State.ETowerType)
        {
            case ETowerType.None:
                break;
            case ETowerType.ArrowTower:
                _bulletPool = PoolManager.Instance._arrowBulletPool;
                break;
            case ETowerType.FireTower:
                _bulletPool = PoolManager.Instance._fireBulletPool;
                break;
            case ETowerType.IceTower:
                _bulletPool = PoolManager.Instance._iceBulletPool;
                break;
        }
    }

    private void Update()
    {
        if (_monsterList.Count == 0) return;

        LookAtTarget();

        if (_tower.State == null) return;

        _curTime += Time.deltaTime;

        if(_curTime > _tower.State.AtkSpeed)
        {
            if(_bulletPool == null)
            {
                Debug.LogError("총알이 없습니다.");
                return;
            }

            _bulletPool.GetObject().Init(_tower, _monsterList[0]);

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
            if (other.TryGetComponent<Monster>(out Monster monster))
            {
                _monsterList.Add(monster);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & _targetLayerMask.value) != 0)
        {
            if (other.TryGetComponent<Monster>(out Monster monster))
            {
                _monsterList.RemoveAt(0);
            }
        }
    }

    private void CacheComponents()
    {
        _tower = GetComponent<Tower>();
    }
}