using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAblity
{
    protected string _skillname;
    protected EPlayerSkillType _ePlayerSkillType;
    protected float _skillDuration;
    protected float _skillDamage;
    protected string _skillexplanation;

    public string Name => _skillname;
    public EPlayerSkillType EPlayerSkillType => _ePlayerSkillType;
    public float SkillDuration => _skillDuration;
    public float SkillDamage => _skillDamage;
    public string Skillexplanation => _skillexplanation;

    public SkillAblity(string skillname, string skillexplanation, float skillDamage, float skillDuration)
    {
        _skillname = skillname;
        _skillexplanation = skillexplanation;
        _skillDamage = skillDamage;
        _skillDuration = skillDuration;
   
    }
}
