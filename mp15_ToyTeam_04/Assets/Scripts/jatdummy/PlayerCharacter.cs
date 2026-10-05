using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 3.5f;
    [SerializeField] private float _rotationSpeed = 10f;

    [SerializeField] private Collider _ground;
    [SerializeField] private float _edgePadding = 0.3f;

    [SerializeField] private PlayerCamera _playerCamera;  // 카메라 전환

    
    private Animator _anim;
    private CharacterController _controller;
    private Transform _cam;
    private LayerMask _towerTileLayer;
    private float _interactDistance = 1f;

    public TowerAbility ArrowTower = new TowerAbility(ETowerType.ArrowTower);
    public TowerAbility FireTower = new TowerAbility(ETowerType.FireTower);
    public TowerAbility IceTower = new TowerAbility(ETowerType.IceTower);

    // public TowerState ArrowTower = new ArrowTower();
    // public TowerState FireTower = new FireTower();
    // public TowerState IceTower = new IceTower();





    private void Start()
    {
        _anim = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();

        // 카메라 기준 이동
        if (Camera.main != null)
            _cam = Camera.main.transform;

        // 바닥 찾기
        if (_ground == null)
        {
            GameObject groundObject = GameObject.Find("Ground"); // 직접 인스펙터에 넣어주거나
            if (groundObject != null) _ground = groundObject.GetComponent<Collider>(); // 이름을 Ground로 바꾸시면 됩니다.

        }

        // _towerTilelayer = LayerMask.NameToLayer("TowerTile"); 정신차려!!
        _towerTileLayer = LayerMask.GetMask("TowerTile");

    }

    private void Update()
    {
        bool isTopView = _playerCamera != null && _playerCamera.IsTopView;

        if (isTopView)
        {
            // 탑뷰: 이동 멈춤, 마우스로만 조작
            UpdateAnimation(false);

            // 마우스 상호작용
            if (Input.GetMouseButtonDown(0))
                TopViewInteract();
        }
        else
        {
            Move();

            // 캐릭터 상호작용
            if (Input.GetKeyDown(KeyCode.E))
                PlayerInteract();
        }

        // 카메라 전환
        if (Input.GetKeyDown(KeyCode.Tab) && _playerCamera != null)
            _playerCamera.SwitchCamera();

        // 플레이서 스킬 사용.
        if (Input.GetKeyDown(KeyCode.Q) && PlayerManager.Instance != null)
            PlayerManager.Instance.UseSkill();

    }


    // 캐릭터 이동
    private void Move()
    {
        Vector3 move = CameraMove();

        bool isMoving = move != Vector3.zero;
        UpdateAnimation(isMoving);

        // 회전
        if (isMoving)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }

        // 맵 경계 설정
        if (_ground != null)
            move = LimitToGround(move);

        // 이동 / 중력 / 충돌
        _controller.SimpleMove(move * _moveSpeed);
    }

    // 카메라 전환에 따른 이동
    private Vector3 CameraMove()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 input = new Vector3(horizontal, 0f, vertical).normalized;

        // 카메라 좌우 각도만큼 돌리기
        if (_cam == null) return input;
        return Quaternion.Euler(0f, _cam.eulerAngles.y, 0f) * input;
    }

    // 캐릭터 상호작용
    public void PlayerInteract()
    {
        Vector3 frontPoint = transform.position + transform.forward * _interactDistance;
        Vector3 rayStart = frontPoint + Vector3.up * 3f;
        
        if (!Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, 6f, _towerTileLayer, QueryTriggerInteraction.Ignore)) return;

        Tile tile = GetTowerTile(hit.collider);
        if (tile != null)
            PlayerManager.Instance.SelectTile(tile);
    }

    // 탑뷰 마우스 상호작용
    public void TopViewInteract()
    {
        if (PlayerManager.Instance == null || Camera.main == null) return;
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;   // UI 위 클릭 무시

        // 마우스로 클릭한 설치 타일 (6번 레이어만)
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, 200f, _towerTileLayer, QueryTriggerInteraction.Ignore)) return;

        Tile tile = GetTowerTile(hit.collider);
        if (tile != null)
            PlayerManager.Instance.SelectTile(tile);
    }

    // 레이에 맞은 콜라이더에서 설치 타일 꺼내기 (Tile이 없거나 설치 타일이 아니면 null)
    private Tile GetTowerTile(Collider hitCollider)
    {
        Tile tile = hitCollider.GetComponentInParent<Tile>();
        if (tile == null) return null;

        if (tile.ETileType != ETileType.Tower)
        {
            // 테스트 로그
            Debug.Log($"{tile.name}: 타일 속성이 Tower가 아님 ({tile.ETileType})");
            return null;
        }
        return tile;
    }

    // 캐릭터 이동 애니메이션
    private void UpdateAnimation(bool isMoving)
    {
        _anim.SetBool("isRunning", isMoving);
    }

    // 맵 경계 바운더리
    private Vector3 LimitToGround(Vector3 dir)
    {
        Bounds groundBounds = _ground.bounds;
        Vector3 next = transform.position + dir * _moveSpeed * Time.deltaTime;

        if (next.x < groundBounds.min.x + _edgePadding || next.x > groundBounds.max.x - _edgePadding) dir.x = 0f;
        if (next.z < groundBounds.min.z + _edgePadding || next.z > groundBounds.max.z - _edgePadding) dir.z = 0f;

        return dir;
    }
}