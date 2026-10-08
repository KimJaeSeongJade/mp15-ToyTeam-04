using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook _playerCam;
    [SerializeField] private GameObject _topViewCam;

    [SerializeField] private float _panSpeed = 15f;     // 기본 이동 속도
    [SerializeField] private float _smoothSpeed = 10f;  // 클수록 딱딱, 작을수록 부드럽게

    [SerializeField] private float _zoomSpeed = 5f;     // 휠 한 칸에 움직이는 높이
    [SerializeField] private float _minHeight = 15f;    // 최대 확대 (가장 낮은 높이)
    [SerializeField] private float _maxHeight = 45f;    // 최대 축소 (가장 높은 높이)
    [SerializeField] private bool _zoomToCursor = true; // 마우스가 가리키는 곳으로 확대

    // 현재 맵 바닥 
    private Collider _ground;
    // 탑뷰일시
    private bool _isTopView;
    // 로비일시
    private bool _isPlaying;
    // Ui 팝업 일시
    private bool _isUIMode;
    public bool IsUIMode => _isUIMode;

    private Vector3 _topViewTarget;     // 탑뷰 카메라가 가려는 위치
    private Vector3 _topViewStartPosition; // 탑뷰 카메라 위치 기억
    
    // 지금 탑뷰인지
    public bool IsTopView => _isTopView;

    public void Start()
    {

        _topViewTarget = _topViewCam.transform.position;
        _topViewStartPosition = _topViewCam.transform.position;
        //CameraRoutine();

        // 로비 일 때. 아닐 때 카메라.
        if (_isPlaying)
        {
            // _topViewTarget = _topViewCam.transform.position;

            CameraRoutine();
        }

        else LobbyStopCamera();

    }

    public void Update()
    {

        if (!_isPlaying) return;
        
        if (!_isTopView) return; 

        TopViewMove();
        TopViewZoom();

        // 목표 위치로 부드럽게 이동
        Transform cam = _topViewCam.transform;
        cam.position = Vector3.Lerp(cam.position, _topViewTarget, _smoothSpeed * Time.deltaTime);

    }
    public void LobbyStartCamera()
    {
        _isPlaying = true;
        _isUIMode = false;
        _isTopView = false;
        _topViewTarget = _topViewStartPosition;
        _topViewCam.transform.position = _topViewStartPosition;
        CameraRoutine();
    }
    public void LobbyStopCamera()
    {
        _isPlaying = false;
        _isUIMode = false;
        _isTopView = true;
        CameraRoutine();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // 카메라 전환 tap 입력하면 변경
    public void SwitchCamera()
    {
        _isTopView = !_isTopView;
        CameraRoutine();
    }

    // 팝업시 3인칭에서 커서도 풀고 카메라 회전도 멈추고 하..
    public void SetUIMode(bool isUIMode)
    {
        _isUIMode = isUIMode;
        CameraRoutine();
    }

    // 카메라 전환 시네머신은 신이야
    private void CameraRoutine()
    {
        _topViewCam.SetActive(_isTopView);

        // 탑뷰 or ui모드시 커서 자유롭게 하려면~
        bool isCursorFree = _isTopView || _isUIMode;

        // 탑뷰일시 3인칭 카메라 마우스 커서 케어
        // 몇번 바꾸는지 모르겠네..
        // _playerCam.m_XAxis.m_InputAxisName = _isTopView ? "" : "Mouse X";
        // _playerCam.m_YAxis.m_InputAxisName = _isTopView ? "" : "Mouse Y";

        _playerCam.m_XAxis.m_InputAxisName = isCursorFree ? "" : "Mouse X";
        _playerCam.m_YAxis.m_InputAxisName = isCursorFree ? "" : "Mouse Y";
        _playerCam.m_XAxis.m_InputAxisValue = 0f;
        _playerCam.m_YAxis.m_InputAxisValue = 0f;

        // 시점별 커서 잠그고 보이기
        //Cursor.lockState = _isTopView ? CursorLockMode.None : CursorLockMode.Locked;
        //Cursor.visible = _isTopView;
        Cursor.lockState = isCursorFree ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isCursorFree;

    }

    // 탑뷰 이동 wasd로
    private void TopViewMove()
    {
        Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        if (dir == Vector3.zero) return;

        // 줌에 따라 속도 조절
        float speed = _panSpeed * (_topViewTarget.y / _maxHeight);

        _topViewTarget += dir.normalized * speed * Time.deltaTime;
        ClampToGround();
    }

    // 탑뷰시 마우스 휠로 거리 조절
    private void TopViewZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll == 0f) return;

        float groundY = _ground != null ? _ground.bounds.max.y : 0f;
        float oldHeight = _topViewTarget.y;
        float newHeight = Mathf.Clamp(oldHeight - scroll * 10f * _zoomSpeed, _minHeight, _maxHeight);

        // 커서 쪽으로 확대: 높이가 줄어든 비율만큼 커서 위치로 다가가기
        if (_zoomToCursor && TryGetMouseGroundPoint(groundY, out Vector3 point))
        {
            float t = 1f - (newHeight - groundY) / (oldHeight - groundY);
            _topViewTarget.x += (point.x - _topViewTarget.x) * t;
            _topViewTarget.z += (point.z - _topViewTarget.z) * t;
        }

        _topViewTarget.y = newHeight;
        ClampToGround();
    }

    // 마우스가 가리키는 바닥 위치 구하기 (나중에 타워 타일 스크립트 보고 변경 예정의 예정)
    private bool TryGetMouseGroundPoint(float groundY, out Vector3 point)
    {
        point = Vector3.zero;
        if (Camera.main == null) return false;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0f, groundY, 0f));

        if (!groundPlane.Raycast(ray, out float distance)) return false;
        point = ray.GetPoint(distance);
        return true;
    }
    public void SetGround(Collider ground)
    {
        _ground = ground;
    }
    // 탑뷰 카메라가 맵(Ground) 밖으로 못 나가게
    private void ClampToGround()
    {
        if (_ground == null) return;

        Bounds groundBounds = _ground.bounds;
        _topViewTarget.x = Mathf.Clamp(_topViewTarget.x, groundBounds.min.x, groundBounds.max.x);
        _topViewTarget.z = Mathf.Clamp(_topViewTarget.z, groundBounds.min.z, groundBounds.max.z);
    }
}
