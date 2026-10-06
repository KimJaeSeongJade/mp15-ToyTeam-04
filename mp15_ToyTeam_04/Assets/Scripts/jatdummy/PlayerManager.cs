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

        /* 로비 전환 케어 테스트용
        if (Input.GetKeyDown(KeyCode.U))
        {
            if (MapManager.Instance != null)
                MapManager.Instance.ShowBattleMap();
            else
                Debug.Log("경계가 없는데?");

            OnPlayer();
        }
        if (Input.GetKeyDown(KeyCode.I)) OffPlayer();
        */
    }


    [SerializeField]private PlayerCamera _playerCamera;

    // 맵 켜질 때 - 끄고 위치 옮기고 다시 켜기
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

        // Debug.Log($"바닥: {(ground != null ? ground.name : "없음")}");   // 확인용
    }

    // 로비 갈 때 - 캐릭터 끄기
    public void OffPlayer()
    {
        _playerCamera.LobbyStopCamera();
        _character.gameObject.SetActive(false);
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

    // 카메라 커서 끄기
    // 키면 0 1.5 0 
    // 끄면 위치 초기화
    // 로비 에서 막고 다시 전투화면 켜고.

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


    public void UseSkill()
    {

    }

    public void SelectTile(Tile tile)
    {
        _selectedTile = tile;
    }

}
