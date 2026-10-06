using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public enum EPlayerSkill
{
    None = -1,
    TimeFreeze = 0,       
    Disaster=1
}
*/

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    private PlayerCharacter _character;
    private Tile _selectedTile;   // 팝업 대상 타일
    public Tile SelectedTile => _selectedTile; 

    // 일단 60초로 고정.
    private const float SKILL_COOL_TIME = 60f; 
    // 남은 시간
    
    private float _skillCoolTimer;
    public float SkillCoolTimer => _skillCoolTimer; // Ui 표시해야지.
    // [SerializeField] private EPlayerSkill _equipSkill = EPlayerSkill.TimeFreeze;
    // 시간 동결 지속 시간 (초)
    // public float FreezeDuration = 3f;     // 시간 동결 지속 시간 (초)
    // 20 으로 보내기
    // public float DisasterPer = 20f;   // 20 으로 보내기
    
    private Dictionary<ETowerType, TowerState> _towerStates = new Dictionary<ETowerType, TowerState>
    {
        { ETowerType.ArrowTower, new ArrowTower() },
        { ETowerType.FireTower,  new FireTower()  },
        { ETowerType.IceTower,   new IceTower()   }
    };

    // 타워 능력
    private Dictionary<ETowerType, TowerAbility> _towerAbilities = new Dictionary<ETowerType, TowerAbility>
    {
        { ETowerType.ArrowTower, new TowerAbility(ETowerType.ArrowTower) },
        { ETowerType.FireTower,  new TowerAbility(ETowerType.FireTower)  },
        { ETowerType.IceTower,   new TowerAbility(ETowerType.IceTower)   }
    };

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _character = GetComponent<PlayerCharacter>();
    }

    public void Update()
    {
        if (_skillCoolTimer > 0f) 
        {
            _skillCoolTimer -= Time.deltaTime;
        }
    }

    // 타워 스텟 가져다가
    public TowerState GetTowerState(ETowerType type)
    {
        _towerStates.TryGetValue(type, out TowerState state);
        return state;
    }

    // 어빌리티 가져다가
    public TowerAbility GetTowerAbility(ETowerType type)
    {
        _towerAbilities.TryGetValue(type, out TowerAbility ability);
        return ability;
    }

    /* 나중에 확인
    public void EquipSkill(EPlayerSkill skill)
    {
        _equipSkill = skill;
    }

    public void UseSkill()
    {
        if (_skillCoolTimer > 0f) return; // 쿨타임 일시 x
        if (_equipSkill == EPlayerSkill.None) return;   // 장착 스킬 없음

        // 사용 스킬효과. 코드
        ApplySkillEffect();

        // 사용후 쿨타임 초기화
        _skillCoolTimer = SKILL_COOL_TIME;
    }

    private void ApplySkillEffect()
    {
        foreach (Monster monster in FindObjectsOfType<Monster>())   // 활성화된 몬스터만 (풀에 들어간 몬스터는 제외)
        {
            if (_equipSkill == EPlayerSkill.TimeFreeze)
                monster.ApplyTimeFreeze(FreezeDuration);
            else if (_equipSkill == EPlayerSkill.Disaster)
                monster.SkillDamage((int)DisasterPer); // 이거 int로만 들어가는데 맞나요?
        }
    }
    // freezeDuration, DisasterPer  Monster 코드에서 확인하기.
    */

    public void SelectTile(Tile tile)
    {
        _selectedTile = tile;
    }

}
