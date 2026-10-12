using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTower : TowerState
{
    /// <summary> 타워 기본 정보 </summary>
    public FireTower()
    {
        _name = "파이어 타워";
        _eTowerType = ETowerType.FireTower;
        _atk = 20;
        _atkSpeed = 3f;
        _detectionRange = 5f;
        _explanation = "느린 공격 속도와 강한 공격력, 넓은 공격 범위를 가진 타워.";
        _installCost = 200;
        _basicLevel = 1;
        _curLevel = _basicLevel;
        _firstUpgradeCost = 500;
        _secondUpgradeCost = 1000;

        _upgradeData = new TowerUpgradeData(5, 0.1f, 0.5f, 5);
        _affect = new BulletAffect(EBulletAffectType.Burn, true, 10, 2);
    }
}
