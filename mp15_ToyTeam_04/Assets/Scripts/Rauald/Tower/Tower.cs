using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    protected string _name;
    protected int _atk;
    protected float _atkSpeed;
    protected float _detectionRange;
    protected string _explanation;
    protected int _installCost;
    protected int _enforceFirstCost;
    protected int _enforceSecondCost;


    /// <summary> 타워 이름 </summary>
    public string Name => _name;
    /// <summary> 타워 기본 공격력 </summary>
    public int Atk => _atk;
    /// <summary> 타워 기본 공격 속도 </summary>
    public float AtkSpeed => _atkSpeed;
    /// <summary> 타워 기본 탐색(공격) 범위 </summary>
    public float DetectionRange => _detectionRange;
    /// <summary> 타워 정보 </summary>
    public string Explanation => _explanation;
    /// <summary> 타워 기본 설치 비용 </summary>
    public int InstallCost => _installCost;
    /// <summary> 타워 기본 강화 1 -> 2단계 비용 </summary>
    public int EnforceFirstCost => _enforceFirstCost;
    /// <summary> 타워 기본 강화 2 -> 3단계 비용 </summary>
    public int EnforceSecondCost => _enforceSecondCost;


    public abstract int TowerInstallCost(bool isAbility);
    public abstract int TowerEnforceFirstCost(bool isAbility);
    public abstract int TowerEnforceSecondCost(bool isAbility);
}