using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTower : Tower
{
    /// <summary> 타워 기본 정보 </summary>
    public ArrowTower()
    {
        _name = "애로우 타워";
        _eTowerType = ETowerType.ArrowTower;
        _atk = 10;
        _atkSpeed = 0.5f;
        _detectionRange = 5f;
        _explanation = "평범한 공격 속도와 공격력, 평범한 공격 범위를 가진 타워.";
        _installCost = 100;
        _basicLevel = 1;
        _curLevel = _basicLevel;
        _firstUpgradeCost = 300;
        _secondUpgradeCost = 700;
    }
}