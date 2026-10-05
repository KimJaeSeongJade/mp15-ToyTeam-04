using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAffect
{
    private EBulletAffectType _eBulletAffectType;
    private bool _isPer;
    private int _affectValue;
    private float _duration;

    public EBulletAffectType EBulletAffectType => _eBulletAffectType;
    public bool IsPer => _isPer;
    public int AffectValue => _affectValue;
    public float Duration => _duration;


    public BulletAffect(EBulletAffectType eBulletAffectType, bool isPer, int affectValue, float duration)
    {
        _eBulletAffectType = eBulletAffectType;
        _isPer = isPer;
        _affectValue = affectValue;
        _duration = duration;
    }
}