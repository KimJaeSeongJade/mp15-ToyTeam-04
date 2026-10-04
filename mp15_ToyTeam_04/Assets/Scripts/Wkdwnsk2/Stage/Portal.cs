using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private LayerMask _monsterLayer;
    [SerializeField] private Stage _stage;

    private void OnTriggerEnter(Collider other)
    {
        // Monster 레이어인지 확인
        if ((_monsterLayer.value & (1 << other.gameObject.layer)) == 0)
        {
            return;
        }

        Monster monster = other.GetComponentInParent<Monster>();

        if (monster == null)
        {
            return;
        }

        // 포탈에 도착 알림
        _stage.MonsterReachedGoal(monster);

        // 몬스터 삭제
        monster.MonsterDelete();
    }
}