using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour, IPoolable
{
    [SerializeField] private GameObject[] _towerObj;
    private Dictionary<EAbilityType, Ability> _dicAbility;
    [SerializeField] private SphereCollider _col;

    private Ability _ability;
    private TowerState _state;

    /// <summary> 타워 정보 </summary>
    public TowerState State => _state;

    public void Test(EAbilityType eAbilityType)
    {
        _dicAbility[eAbilityType].AbilityLearn();
    }

    /// <summary> 타워 설치 비용 계산 </summary>
    /// <param name="isAbility"> 특성에 타워 설치 비용 감소를 습득 했는지 여부 </param>
    /// <returns> 계산된 설치 비용 </returns>
    public int TowerInstallCost()
    {
        _ability = _dicAbility[EAbilityType.DecreaseInstallCost];
        return (int)(_state.InstallCost * (1 * (100 - (_ability.IsLearn ? _ability.Value : 0f)) / 100));
    }

    /// <summary> 타워 설치 </summary>
    public void TowerBasicInstall(TowerState state, TowerAbility towerAbility)
    {
        _state = state;
        _dicAbility = towerAbility.DicAbility;

    }

    /// <summary> 타워 철거 비용 계산 </summary>
    /// <returns> 계산된 철거 비용 </returns>
    public int TowerRemovalCost()
    {
        int removalCost = _state.InstallCost / 2;

        if(_state.CurLevel == 2)
        {
            removalCost += (_state.FirstUpgradeCost / 2);
        }

        if(_state.CurLevel == 3)
        {
            removalCost += (_state.SecondUpgradeCost / 2);
        }

        return removalCost;
    }

    /// <summary> 타워 철거 </summary>
    public void TowerRemoval()
    {
        _dicAbility = null;
        _towerObj[_state.CurLevel - 1].SetActive(false);
    }

    /// <summary> 타워 공격력 </summary>
    /// <returns> 계산된 타워 공격력 </returns>
    public int TowerAtk()
    {
        _ability = _dicAbility[EAbilityType.IncreaseAtk];
        return _state.CurAtk() + (_ability.IsLearn ? (int)_ability.Value : 0);
    }

    /// <summary> 타워 공격 속도 </summary>
    /// <returns> 계산된 타워 공격 속도 </returns>
    public float TowerAtkSpeed()
    {
        _ability = _dicAbility[EAbilityType.IncreaseAtkSpeed];
        return _state.AtkSpeed - (_ability.IsLearn ? _ability.Value : 0);
    }

    /// <summary> 타워 탐색 범위 </summary>
    /// <returns> 계산된 타워 탐색 범위 </returns>
    public float TowerDetectionRange()
    {
        _ability = _dicAbility[EAbilityType.IncreaseDetectionRange];
        return _state.DetectionRange + (_ability.IsLearn ? _ability.Value : 0);
    }

    /// <summary> 타워 강화 비용 </summary>
    /// <returns> 계산된 강화 비용 </returns>
    public int TowerUpgradeCost()
    {
        _ability = _dicAbility[EAbilityType.DecreaseUpgradeCost];
        return (int)((_state.CurLevel == 1 ? _state.FirstUpgradeCost : _state.SecondUpgradeCost) * (1 * (100 - (_ability.IsLearn ? _ability.Value : 0f)) / 100));
    }

    /// <summary> 타워 업그레이드 </summary>
    public void TowerUpgrade()
    {
        if (_state.CurLevel == 3) return;

        _towerObj[_state.CurLevel - 1].SetActive(false);
        _state.TowerUpgrade();
        _towerObj[_state.CurLevel - 1].SetActive(true);
    }

    public void TowerClear()
    {
        _state = null;
        _dicAbility = null;
        foreach(GameObject obj in _towerObj)
        {
            obj.SetActive(false);
        }
    }

    public void OnSpawn()
    {
        _towerObj[_state.BasicLevel - 1].SetActive(true);
        _col.radius = TowerDetectionRange();
    }

    public void OnDespawn()
    {
        GoldManager.Instance.AddGold(TowerRemovalCost());
        TowerRemoval();
    }
}