using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private Collider ground;
    [SerializeField] private float edgePadding = 0.3f;

    private Animator anim;
    private CharacterController controller;
    private Transform cam;

    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

        // 바닥 찾기
        if (ground == null)
        {
            GameObject groundObject = GameObject.Find("Ground"); // 직접 인스펙터에 넣어주거나
            if (groundObject != null) ground = groundObject.GetComponent<Collider>(); // 이름을 Ground로 바꾸시면 됩니다.
        }
    }


    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // 입력 방향
        Vector3 move = new Vector3(horizontal, 0f, vertical).normalized;

        bool isMoving = move != Vector3.zero;
        anim.SetBool("isRunning", isMoving);

        // 회전
        if (isMoving)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // 맵 경계 설정
        if (ground != null)
            move = LimitToGround(move);

        // 이동 + 중력 + 충
        controller.SimpleMove(move * moveSpeed);
    }

    // 맵 경계 바운더리 함수
    Vector3 LimitToGround(Vector3 dir)

    {
        Bounds groundBounds = ground.bounds;
        Vector3 next = transform.position + dir * moveSpeed * Time.deltaTime;

        if (next.x < groundBounds.min.x + edgePadding || next.x > groundBounds.max.x - edgePadding) dir.x = 0f;
        if (next.z < groundBounds.min.z + edgePadding || next.z > groundBounds.max.z - edgePadding) dir.z = 0f;

        return dir;
    }

}