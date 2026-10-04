using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTower : TowerState
{
    /// <summary> 타워 기본 정보 </summary>
    public IceTower()
    {
        _name = "아이스 타워";
        _eTowerType = ETowerType.IceTower;
        _atk = 5;
        _upgradeAtk = 2;
        _atkSpeed = 0.8f;
        _detectionRange = 2f;
        _explanation = "빠른 공격 속도와 약한 공격력, 준수한 공격 범위를 가진 타워.";
        _installCost = 100;
        _basicLevel = 1;
        _curLevel = _basicLevel;
        _firstUpgradeCost = 300;
        _secondUpgradeCost = 700;
    }
}