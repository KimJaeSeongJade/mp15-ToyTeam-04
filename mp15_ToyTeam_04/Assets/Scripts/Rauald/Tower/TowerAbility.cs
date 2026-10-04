using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAbility
{
    private Dictionary<EAbilityType, Ability> _dicAbility = new();

    public Dictionary<EAbilityType, Ability> DicAbility => _dicAbility;

    public TowerAbility(ETowerType eTowerType)
    {
        switch (eTowerType)
        {
            case ETowerType.None:
                break;
            case ETowerType.ArrowTower:
                _dicAbility.Add(EAbilityType.IncreaseAtk, new Ability("예리한 화살촉", "화살촉을 날카롭게 다듬어 공격력이 증가합니다.", 5, 100));
                _dicAbility.Add(EAbilityType.IncreaseAtkSpeed, new Ability("신속한 장전", "화살을 빠르게 장전하여 공격 속도가 증가합니다.", 0.3f, 150));
                _dicAbility.Add(EAbilityType.IncreaseDetectionRange, new Ability("고탄성 시위", "시위를 개량하여 공격 사거리가 증가합니다.", 1f, 200));
                _dicAbility.Add(EAbilityType.DecreaseInstallCost, new Ability("보급품 증가", "건설에 필요한 보급품이 증가하여 설치 비용이 감소합니다.", 10, 300));
                _dicAbility.Add(EAbilityType.DecreaseUpgradeCost, new Ability("제작의 달인", "제작 기술이 달인급이 되어 강화 비용이 감소합니다.", 10, 300));
                break;
            case ETowerType.FireTower:
                _dicAbility.Add(EAbilityType.IncreaseAtk, new Ability("작열하는 불꽃", "불꽃의 온도를 더 뜨겁게 공격력이 증가합니다.", 10, 150));
                _dicAbility.Add(EAbilityType.IncreaseAtkSpeed, new Ability("점화", "불꽃을 빠르게 태워 공격 속도가 증가합니다.", 0.5f, 200));
                _dicAbility.Add(EAbilityType.IncreaseDetectionRange, new Ability("추적하는 불꽃", "불꽃이 더 강하게 타올라 공격 사거리가 증가합니다.", 2f, 250));
                _dicAbility.Add(EAbilityType.DecreaseInstallCost, new Ability("보급품 증가", "건설에 필요한 보급품이 증가하여 설치 비용이 감소합니다.", 10, 350));
                _dicAbility.Add(EAbilityType.DecreaseUpgradeCost, new Ability("제작의 달인", "제작 기술이 달인급이 되어 강화 비용이 감소합니다.", 10, 350));
                break;
            case ETowerType.IceTower:
                _dicAbility.Add(EAbilityType.IncreaseAtk, new Ability("절대 영도", "얼음의 온도를 더 차갑게 만들어 공격력이 증가합니다.", 3, 150));
                _dicAbility.Add(EAbilityType.IncreaseAtkSpeed, new Ability("내려앉은 한기", "한기로 인해 얼음을 더 빠르게 만들어 공격 속도가 증가합니다.", 0.3f, 200));
                _dicAbility.Add(EAbilityType.IncreaseDetectionRange, new Ability("응축된 얼음", "얼음을 응축시켜 공격 범위가 증가합니다.", 0.5f, 250));
                _dicAbility.Add(EAbilityType.DecreaseInstallCost, new Ability("보급품 증가", "건설에 필요한 보급품이 증가하여 설치 비용이 감소합니다.", 10, 400));
                _dicAbility.Add(EAbilityType.DecreaseUpgradeCost, new Ability("제작의 달인", "제작 기술이 달인급이 되어 강화 비용이 감소합니다.", 10, 400));
                break;
        }
    }
}