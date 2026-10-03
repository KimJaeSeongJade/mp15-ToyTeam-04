using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [SerializeField] private GameObject[] _towerObj;

    protected string _name;
    protected ETowerType _eTowerType;
    protected int _atk;
    protected float _atkSpeed;
    protected float _detectionRange;
    protected string _explanation;
    protected int _installCost;
    protected int _basicLevel;
    protected int _curLevel;
    protected int _firstUpgradeCost;
    protected int _secondUpgradeCost;


    /// <summary> 타워 이름 </summary>
    public string Name => _name;
    /// <summary> 타워 타입 </summary>
    public ETowerType ETowerType => _eTowerType;
    /// <summary> 타워 정보 </summary>
    public string Explanation => _explanation;
    /// <summary> 현재 강화 단계 </summary>
    public int CurLevel => _curLevel;

    /// <summary> 타워 설치 비용 계산 </summary>
    /// <param name="isAbility"> 특성에 타워 설치 비용 감소를 습득 했는지 여부</param>
    /// <returns> 계산된 설치 비용 </returns>
    public int TowerInstallCost(bool isAbility)
    {
        return (_installCost) * (isAbility ? 2 : 1);
    }

    /// <summary> 타워 설치 </summary>
    public void TowerBasicInstall()
    {
        _towerObj[_basicLevel - 1].SetActive(true);
    }

    /// <summary> 타워 철거 비용 계산 </summary>
    /// <returns> 계산된 철거 비용 </returns>
    public int TowerRemovalCost()
    {
        int removalCost = _installCost / 2;

        if(_curLevel == 2)
        {
            removalCost += (_firstUpgradeCost / 2);
        }

        if(_curLevel == 3)
        {
            removalCost += (_secondUpgradeCost / 2);
        }

        return removalCost;
    }

    /// <summary> 타워 철거 </summary>
    public void TowerRemoval()
    {
        _towerObj[_curLevel - 1].SetActive(false);
    }

    /// <summary> 타워 공격력 </summary>
    /// <param name="isAbility"> 특성에 타워 공격 증가를 습득 했는지 여부 </param>
    /// <returns> 계산된 타워 공격력 </returns>
    public int TowerAtk(bool isAbility)
    {
        return _atk * (isAbility ? 2 : 1);
    }

    /// <summary> 타워 공격 속도 </summary>
    /// <param name="isAbility"> 특성에 타워 공격 속도 증가를 습득 했는지 여부 </param>
    /// <returns> 계산된 타워 공격 속도 </returns>
    public float TowerAtkSpeed(bool isAbility)
    {
        return _atkSpeed * (isAbility ? 2 : 1);
    }

    /// <summary> 타워 탐색 범위 </summary>
    /// <param name="isAbility"> 특성에 타워 탐색 범위 증가를 습득 했는지 여부 </param>
    /// <returns> 계산된 타워 탐색 범위 </returns>
    public float TowerDetectionRange(bool isAbility)
    {
        return _detectionRange * (isAbility ? 2 : 1);
    }

    /// <summary> 타워 강화 비용 </summary>
    /// <param name="isAbility"> 특성에 타워 강화 비용 감소를 습득 했는지 여부 </param>
    /// <returns> 계산된 강화 비용 </returns>
    public int TowerUpgradeCost(bool isAbility)
    {
        return (_curLevel == 1 ? _firstUpgradeCost : _secondUpgradeCost) * (isAbility ? 0 : 1);
    }

    /// <summary> 타워 업그레이드 </summary>
    public void TowerUpgrade()
    {
        if (_curLevel == 3) return;

        _towerObj[_curLevel - 1].SetActive(false);
        _curLevel++;
        _towerObj[_curLevel - 1].SetActive(true);
    }
}