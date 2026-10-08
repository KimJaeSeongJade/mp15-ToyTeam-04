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
                _dicSkillAblity.Add(EPlayerSkillType.TimeFreeze, new SkillAblity("시간을 멈춘다", "시간을 멈춘다고", 0f, 10f));
                break;
            case EPlayerSkillType.NaturalDisaster:
                _dicSkillAblity.Add(EPlayerSkillType.NaturalDisaster, new SkillAblity("세상에 비호감 딱 두명있데 뚜비두밥", "어떻게 두명이나 뚜비두밥", 20f, 0f));
                break;
        }
    }
}