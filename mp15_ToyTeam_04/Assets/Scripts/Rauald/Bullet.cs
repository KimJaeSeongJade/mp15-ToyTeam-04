using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private LayerMask _targetLayerMask;
    [SerializeField] private float _duration = 0.5f;
    [SerializeField] private float _maxHeight = 0.5f;

    private BulletAffect _bulletAffect;
    private Transform _target;
    private int _atk;
    private IDamageable _iDamage;

    public void Init(BulletAffect bulletAffect, Transform target, int atk, IDamageable iDamage)
    {
        _bulletAffect = bulletAffect;
        _target = target;
        _atk = atk;
        _iDamage = iDamage;

        StartCoroutine(Shooting());
    }

    /// <summary> 표적을 향해 날라가기 </summary>
    private IEnumerator Shooting()
    {
        float curTime = 0f;
        Vector3 startPos = transform.position;

        while (curTime < _duration)
        {
            // 공격 도중 적이 사라졌다면
            if (_target == null)
            {
                // 오브젝트 풀에 다시 넣기.
                yield return null;
            }

            // 계산
            curTime += Time.deltaTime;
            float t = curTime / _duration;

            // 몬스터의 현재 위치를 갱신하여 이동
            Vector3 nextPos = Vector3.Lerp(startPos, _target.position, t);

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
        _iDamage.TakeDamage(false, _atk);
        //_iDamage.Affect(_bulletAffect);
    }
}