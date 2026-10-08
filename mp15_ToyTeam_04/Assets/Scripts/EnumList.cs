/// <summary> 타일 타입 </summary>
public enum ETileType
{
    /// <summary> 없음 </summary>
    None = 0,
    /// <summary> 타워 </summary>
    Tower = 1,
    /// <summary> 몬스터 </summary>
    Monster = 2
}

/// <summary> 타워 타입 </summary>
public enum ETowerType
{
    /// <summary> 없음 </summary>
    None = -1,
    /// <summary> 애로우 타워 </summary>
    ArrowTower = 0,
    /// <summary> 파이어 타워 </summary>
    FireTower = 1,
    /// <summary> 아이스 타워 </summary>
    IceTower = 2,
}

/// <summary> 공격 효과 타입 </summary>
public enum EBulletAffectType
{
    /// <summary> 없음 </summary>
    None = -1,
    /// <summary> 타일 태우기 </summary>
    Burn = 0,
    /// <summary> 이속 감소 </summary>
    DecreaseSpeed = 1
}

/// <summary> 특성 타입 </summary>
public enum EAbilityType
{
    /// <summary> 없음 </summary>
    None = -1,
    /// <summary> 공격력 증가 </summary>
    IncreaseAtk = 0,
    /// <summary> 공격 속도 증가 </summary>
    IncreaseAtkSpeed = 1,
    /// <summary> 공격 사거리 증가 </summary>
    IncreaseDetectionRange = 2,
    /// <summary> 설치 비용 감소 </summary>
    DecreaseInstallCost = 3,
    /// <summary> 업그레이드 비용 감소 </summary>
    DecreaseUpgradeCost = 4
}

public enum EPlayerSkillType
{
    /// <summary> 없음 </summary>
    None = -1,
    /// <summary> 시간 정지 </summary>
    TimeFreeze = 0,
    /// <summary> 자연 재해 </summary>
    NaturalDisaster = 1
}