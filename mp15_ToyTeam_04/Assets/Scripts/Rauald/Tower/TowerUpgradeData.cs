using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgradeData
{
    private int _addAtk;
    private float _addAtkSpeed;
    private float _addRange;
    private int _addBulletAffect;

    public int AddAtk => _addAtk;
    public float AddAtkSpeed => _addAtkSpeed;
    public float AddRange => _addRange;
    public int AddBulletAffect => _addBulletAffect;

    public TowerUpgradeData(int addAtk, float addAtkSpeed, float addRange, int addBulletAffect)
    {
        _addAtk = addAtk;
        _addAtkSpeed = addAtkSpeed;
        _addRange = addRange;
        _addBulletAffect = addBulletAffect;
    }
}
