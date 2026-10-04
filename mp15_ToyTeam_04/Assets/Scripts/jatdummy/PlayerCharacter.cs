using UnityEngine;

[RequireComponent(typeof(CharacterController))]
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
                CharacterInteract();
        }

        // 카메라 전환
        if (Input.GetKeyDown(KeyCode.Tab) && _playerCamera != null)
            _playerCamera.SwitchCamera();
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
    private void CharacterInteract()
    {
        Debug.Log("상호작용 키 입력");
    }

    // 탑뷰 마우스 상호작용
    private void TopViewInteract()
    {
        Debug.Log("탑뷰 클릭");
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