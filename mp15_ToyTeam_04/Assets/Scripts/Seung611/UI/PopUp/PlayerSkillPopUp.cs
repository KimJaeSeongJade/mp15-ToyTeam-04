using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillPopUp : MonoBehaviour
{
    [SerializeField] private Button _skill1SelectButton;
    [SerializeField] private Button _skill2SelectButton;
    [SerializeField] private TextMeshProUGUI _skill1Name;
    [SerializeField] private TextMeshProUGUI _skill2Name;

    private string _skill1 = "Time freeze";
    private string _skill2 = "Natural disaster";
    
    private void Start() => SkillName();
    private void OnEnable() => BindButtonEvents();
    
    private void OnDisable() => UnbindButtonEvents();

    private void BindButtonEvents()
    {
        _skill1SelectButton.onClick.AddListener(Skill1Selected);
        _skill2SelectButton.onClick.AddListener(Skill2Selected);
    }
    
    private void UnbindButtonEvents()
    {
        _skill1SelectButton.onClick.RemoveListener(Skill1Selected);
        _skill2SelectButton.onClick.RemoveListener(Skill2Selected);
    }
    
    private void Skill1Selected()
    {
        // Skill1을 눌렀을 때 어떤 동작을 할 지 구현
    }
    
    private void Skill2Selected()
    {
        // Skill2를 눌렀을 때 어떤 동작을 할 지 구현
    }

    private void SkillName()
    {
        _skill1Name.text = $"{_skill1}";
        _skill2Name.text = $"{_skill2}";
    }
}
