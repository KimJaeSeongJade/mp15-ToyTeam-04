using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTower : Tower
{
    public ArrowTower()
    {
        _name = "애로우 타워";
        _atk = 10;
        _atkSpeed = 0.5f;
        _detectionRange = 5f;
        _explanation = "기본 타워. 평범한 공격 속도와 평범한 공격력을 가지고 있다.";
        _installCost = 100;
        _enforceFirstCost = 300;
        _enforceSecondCost = 700;
    }

    public override int TowerEnforceFirstCost(bool isAbility)
    {

        return 0;
    }

    public override int TowerEnforceSecondCost(bool isAbility)
    {

        return 0;
    }

    public override int TowerInstallCost(bool isAbility)
    {

        return 0;
    }
}