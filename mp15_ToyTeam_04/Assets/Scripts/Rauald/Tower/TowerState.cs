using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerState
{
    protected string _name;
    protected ETowerType _eTowerType;
    protected int _atk;
    protected int _upgradeAtk;
    protected float _atkSpeed;
    protected float _detectionRange;
    protected string _explanation;
    protected int _installCost;
    protected int _basicLevel;
    protected int _curLevel;
    protected int _firstUpgradeCost;
    protected int _secondUpgradeCost;

    public string Name => _name;
    public ETowerType ETowerType => _eTowerType;
    public int Atk => _atk;
    public float AtkSpeed => _atkSpeed;
    public float DetectionRange => _detectionRange;
    public string Explanation => _explanation;
    public int InstallCost => _installCost;
    public int BasicLevel => _basicLevel;
    public int CurLevel => _curLevel;
    public int FirstUpgradeCost => _firstUpgradeCost;
    public int SecondUpgradeCost => _secondUpgradeCost;

    public void TowerUpgrade()
    {
        if (_curLevel == 3) return;

        _curLevel++;
    }

    public int CurAtk()
    {
        return (_atk + (_upgradeAtk * (_curLevel - 1)));
    }
}
