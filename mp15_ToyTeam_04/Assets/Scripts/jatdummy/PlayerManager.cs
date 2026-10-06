using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    // 시작위치
    private static readonly Vector3 START_POSITION = new Vector3(0f, 1.5f, 0f);
    [SerializeField] private PlayerCharacter _character;
    
    private Tile _selectedTile;   // 팝업 대상 타일
    public Tile SelectedTile => _selectedTile;
    
    // 타일 선택됐을 때 알림 
    public event Action<Tile> OnTileSelected;


    // 일단 60초로 고정.
    private const float SKILL_COOL_TIME = 60f; 
    // 남은 시간
    
    private float _skillCoolTimer;
    public float SkillCoolTimer => _skillCoolTimer; // Ui 표시해야지.
    
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

    // 타워 최고 레벨 
    private const int MAX_TOWER_LEVEL = 3;

    // 설치할 타워 종류
    private ETowerType _selectedTowerType = ETowerType.ArrowTower;
    
    // ui에서 - - -- - - -- - - 
    public ETowerType SelectedTowerType => _selectedTowerType;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        //_character = GetComponentInChildren<PlayerCharacter>();
    }

    private void Update()
    {
        if (_skillCoolTimer > 0f) 
        {
            _skillCoolTimer -= Time.deltaTime;
        }

         
        // 로비 전환 케어 테스트용
        if (Input.GetKeyDown(KeyCode.U))
        {
            if (MapManager.Instance != null)
                MapManager.Instance.ShowBattleMap();
            else
                Debug.Log("맵 경계가 없는데? ground 확인좀..");

            OnPlayer();
        }
        if (Input.GetKeyDown(KeyCode.I)) OffPlayer();
        // UIMode 테스트 (나중에 삭제)
        if (Input.GetKeyDown(KeyCode.O)) SetUIMode(true);    // 팝업 열림 흉내
        if (Input.GetKeyDown(KeyCode.P)) SetUIMode(false);   // 팝업 닫힘 흉내



    }

    [SerializeField] private PlayerCamera _playerCamera;

    // 맵 켜질 때 끄고 위치 옮기고 다시 켜기
    public void OnPlayer( )
    {
        // 이걸로 부르니까 맵 바닥을 인식을 못하던데... 그래서 추가
        Collider ground = CurrentGround();
        _character.SetGround(ground);
        _playerCamera.SetGround(ground);

        _character.gameObject.SetActive(false);
        _character.transform.position = START_POSITION;
        _character.gameObject.SetActive(true);
        _playerCamera.LobbyStartCamera();

    }

    // 로비 갈 때 캐릭터 끄기
    public void OffPlayer()
    {
        _playerCamera.LobbyStopCamera();
        _character.gameObject.SetActive(false);
        
        // 이 때(플레이어 끌 때) 맵에서 처리할거 있으면 추가하겠습니다.
    }

    private Collider CurrentGround()
    {
        // MapManager가 켠 맵 확인
        if (MapManager.Instance != null && MapManager.Instance._curMap != null)
        {
            return MapManager.Instance._curMap.Ground;
        }
        // 맵 정보 없으면 바닥 없음 ㅋㅋ
        return null;

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

    // 설치할 타워 종류 선택 (UI 타워 선택 버튼에서 호출)
    public void SelectTowerType(ETowerType type)
    {

        _selectedTowerType = type;
    }

    // 설치 전 비용 (설치 비용 감소 특성 반영)
    public int GetInstallCost(ETowerType type)
    {
        Ability discount = _towerAbilities[type].DicAbility[EAbilityType.DecreaseInstallCost];
        float percent = discount.IsLearn ? discount.Value : 0f;
        return (int)(_towerStates[type].InstallCost * (100 - percent) / 100);
    }

    // 설치할 때마다 새로 생성
    private TowerState CreateTowerState(ETowerType type)
    {
        switch (type)
        {
            case ETowerType.ArrowTower: return new ArrowTower();
            case ETowerType.FireTower: return new FireTower();
            case ETowerType.IceTower: return new IceTower();
            default: return null;
        }
    }

    // 타워 설치 (UI 설치 버튼)
    public void InstallTower()
    {
        if (_selectedTile.IsTower) return;
        if (!GoldManager.Instance.UseGold(GetInstallCost(_selectedTowerType))) return;

        Tower tower = PoolManager.Instance._towerPool.GetObject();

        // 타일 윗면에 배치
        Vector3 pos = _selectedTile.transform.position;
        pos.y = _selectedTile.GetComponent<Collider>().bounds.max.y;
        tower.transform.position = pos;

        // 타워 켜기 
        _selectedTile._tower = tower;
        _selectedTile.TileInstallTower(CreateTowerState(_selectedTowerType), GetTowerAbility(_selectedTowerType));
        PoolManager.Instance._towerPool.ActivateObject(tower);
    }

    // 타워 강화 (UI 강화 버튼)
    public void UpgradeTower()
    {
        if (!_selectedTile.IsTower) return;
        Tower tower = _selectedTile._tower;
        if (tower.State.CurLevel >= MAX_TOWER_LEVEL) return;
        if (!GoldManager.Instance.UseGold(tower.TowerUpgradeCost())) return;

        tower.TowerUpgrade();
    }

    // 타워 철거 (UI 철거 버튼)
    public void DemolishTower()
    {
        if (!_selectedTile.IsTower) return;
        // 타일 비우기 전에 잡아두기
        Tower tower = _selectedTile._tower;

        _selectedTile.TileRemovalTower();
        
        PoolManager.Instance._towerPool.ReturnObject(tower);
    }


    public void UseSkill()
    {

    }

    public void SelectTile(Tile tile)
    {
        _selectedTile = tile;

        // 신호 갔으면
        if (OnTileSelected != null)
        {
            // 선택된 타일 줄게ㅋ
            OnTileSelected(tile);
        }
    }
    // UI 팝업 열고 닫을 때 이거 쓰심 됩니다.
    // true : 커서보임 카메라 회전 x  , false : 다시 캐릭터 시점
    public void SetUIMode(bool isOpen)
    {
        _playerCamera.SetUIMode(isOpen);
    }

}
