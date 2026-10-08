using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    [SerializeField] private Image _hpbarImage;
    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] private TextMeshProUGUI _damageText;

    private void Awake() => _damageText.gameObject.SetActive(false);

    private void Update() => LookAtCamera();
    public void MonsterStateUpdate(int hp, int maxHealth, int damage)
    {
        MonsterHealthBar(hp, maxHealth);
        MonsterDamage(damage);
    }
    
    private void MonsterHealthBar(int hp, int maxHealth)
    {
        if (hp > maxHealth) return;
        // Clamp01 소괄호 안에 있는 값이 0작거나 1보다 클 때 0과 1 사이에 있는 수로 만들어줌
        _hpbarImage.fillAmount = Mathf.Clamp01((float)hp/maxHealth);
        _hpText.text = hp + " / " + maxHealth;
    }
    

    private void MonsterDamage(int damage)
    {
        DamageOnable();
        _damageText.text = "- "  + damage.ToString();
        StartCoroutine(DamageTextRoutine());
    }

    private IEnumerator DamageTextRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        DamageDisable();
    }
    
    private void DamageOnable()
    {
        _damageText.gameObject.SetActive(true);
    }

    private void DamageDisable()
    {
        _damageText.gameObject.SetActive(false);
    }

    private void LookAtCamera()
    {
        // Hp바가 카메라를 쳐다보게 함
        // transform.LookAt(Camera.main);
    }
}
