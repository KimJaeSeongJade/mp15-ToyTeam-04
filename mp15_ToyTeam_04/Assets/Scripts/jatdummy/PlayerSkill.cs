using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill
{
    private Dictionary<EPlayerSkillType, SkillAblity> _dicSkillAblity = new();
    public Dictionary<EPlayerSkillType, SkillAblity> DicSkillAblity => _dicSkillAblity;
    public PlayerSkill(EPlayerSkillType ePlayerSkillType)
    {
        switch (ePlayerSkillType)
        {
            case EPlayerSkillType.None:
                break;
            case EPlayerSkillType.TimeFreeze:
                _dicSkillAblity.Add(EPlayerSkillType.TimeFreeze, new SkillAblity("타임스탑", "시간을 멈춘다고", 0, 10f));
                break;
            case EPlayerSkillType.NaturalDisaster:
                _dicSkillAblity.Add(EPlayerSkillType.NaturalDisaster, new SkillAblity("천재지변", "재앙의 헌헌.", 20, 0f));
                break;
        }
    }
}