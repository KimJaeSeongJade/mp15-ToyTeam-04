using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    private PlayerCharacter _character;

    private const float SKILL_COOL_TIME = 60f; // 일단 60초로 다 맞춰둠.
    private float _skillCoolTimer; // 남은 시간

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

    public void UseSkill()
    {
        if (_skillCoolTimer > 0f) return; // 쿨타임 일시 x

        // 사용 스킬효과. 코드

        // 사용후 쿨타임 초기화
        _skillCoolTimer = SKILL_COOL_TIME;
    }

    private void Interact()
    {
        // 이거 두개는
    }

    private void TopViewInteract()
    {
        // 진짜 어케해야하지..
    }


}
