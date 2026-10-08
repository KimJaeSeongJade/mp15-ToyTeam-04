using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour, IPoolable
{
    [SerializeField] private LayerMask _targetLayerMask;
    [SerializeField] private float _duration = 0.5f;
    [SerializeField] private float _maxHeight = 0.5f;

    /// <summary> 발사한 타워 </summary>
    private Tower _tower;
    /// <summary> 공격 효과 </summary>
    private BulletAffect _bulletAffect;
    /// <summary> 타겟 몬스터 </summary>
    private Monster _monster;
    /// <summary> 공격력 </summary>
    private int _atk;

    public void Init(Tower tower, Monster monster)
    {
        _tower = tower;
        _bulletAffect = _tower.State.Affect;
        _monster = monster;
        _atk = _tower.TowerAtk();
    }

    /// <summary> 표적을 향해 날라가기 </summary>
    private IEnumerator Shooting()
    {
        float curTime = 0f;
        Vector3 startPos = transform.position;

        while (curTime < _duration)
        {
            // 공격 도중 적이 사라졌다면
            if (_monster == null)
            {
                // 오브젝트 풀에 다시 넣기.
                ReturnBullet();
                yield break;
            }

            // 계산
            curTime += Time.deltaTime;
            float t = curTime / _duration;

            // 몬스터의 현재 위치를 갱신하여 이동
            Vector3 nextPos = Vector3.Lerp(startPos, _monster.transform.position, t);

            // 포물선으로 그리기 위해 높이 추가
            nextPos.y += 4f * _maxHeight * t * (1f - t);

            // 공격 방향 바라보기
            Vector3 direction = nextPos - transform.position;
            transform.rotation = Quaternion.LookRotation(direction);

            transform.position = nextPos;
            // 시간 경과시
            if (t >= 1f)
                break;

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & _targetLayerMask.value) != 0)
        {
            Hit();
        }
    }

    private void Hit()
    {
        _monster.TakeDamage(false, _atk);
        _monster.BulletAffect(_bulletAffect);
        ReturnBullet();
    }

    private void ReturnBullet()
    {
        switch (_tower.State.ETowerType)
        {
            case ETowerType.None:
                break;
            case ETowerType.ArrowTower:
                PoolManager.Instance._arrowBulletPool.ReturnObject(this);
                break;
            case ETowerType.FireTower:
                PoolManager.Instance._fireBulletPool.ReturnObject(this);
                break;
            case ETowerType.IceTower:
                PoolManager.Instance._iceBulletPool.ReturnObject(this);
                break;
        }
    }

    public void OnSpawn()
    {
        StartCoroutine(Shooting());
    }

    public void OnDespawn()
    {
    }
}