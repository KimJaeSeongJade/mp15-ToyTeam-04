using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTower : Tower
{
    /// <summary> 타워 기본 정보 </summary>
    public FireTower()
    {
        _name = "파이어 타워";
        _eTowerType = ETowerType.FireTower;
        _atk = 20;
        _atkSpeed = 1f;
        _detectionRange = 10f;
        _explanation = "느린 공격 속도와 강한 공격력, 넓은 공격 범위를 가진 타워.";
        _installCost = 200;
        _basicLevel = 1;
        _curLevel = _basicLevel;
        _firstUpgradeCost = 500;
        _secondUpgradeCost = 1000;
    }
}
