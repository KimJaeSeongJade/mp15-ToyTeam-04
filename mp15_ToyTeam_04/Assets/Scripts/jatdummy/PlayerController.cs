using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;

    private Animator anim;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 move = new Vector3(horizontal, 0f, vertical);

        // 이동 일 때
        if (move.sqrMagnitude > 0.01f)
        {
            // Run 애니메이션
            anim.SetBool("isRunning", true);

            // 이동 방향으로 부드럽게 회전
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            // 이동
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
        // 정지
        else
        {
            // Idle 애니메이션
            anim.SetBool("isRunning", false);
        }
    }
}
