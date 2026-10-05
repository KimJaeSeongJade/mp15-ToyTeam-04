using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTest : MonoBehaviour, IDamageable
{
    public void TakeDamage(bool per, int damage)
    {
        Debug.Log($"몬스터가 {damage} 만큼 피해를 입었습니다");
    }
}
