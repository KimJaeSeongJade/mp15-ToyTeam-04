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
    [SerializeField] private Image[] _skillImage;
    [SerializeField] private Sprite[] _skillImageSprites;
    [SerializeField] private Button _escButton;
    
    private Image _sillButtonimage => UIManager.Instance.Window.LobbyWindow._playerSkillButton.image;
    
    private void Start() => SkillNameImage();
    private void OnEnable() => BindButtonEvents();
    
    private void OnDisable() => UnbindButtonEvents();

    private void BindButtonEvents()
    {
        _skill1SelectButton.onClick.AddListener(Skill1Selected);
        _skill2SelectButton.onClick.AddListener(Skill2Selected);
        _escButton.onClick.AddListener(Esc);
    }
    
    private void UnbindButtonEvents()
    {
        _skill1SelectButton.onClick.RemoveListener(Skill1Selected);
        _skill2SelectButton.onClick.RemoveListener(Skill2Selected);
        _escButton.onClick.RemoveListener(Esc);
    }
    
    public void Skill1Selected()
    {
        // Skill1을 눌렀을 때 어떤 동작을 할 지 구현
        gameObject.SetActive(false);
        _sillButtonimage.sprite = _skillImageSprites[0];
        PlayerManager.Instance.EquipSkill(EPlayerSkillType.TimeFreeze);
    }
    
    public void Skill2Selected()
    {
        // Skill2를 눌렀을 때 어떤 동작을 할 지 구현
        gameObject.SetActive(false);
        _sillButtonimage.sprite = _skillImageSprites[1];
        PlayerManager.Instance.EquipSkill(EPlayerSkillType.NaturalDisaster);
    }

    private void SkillNameImage()
    {
        _skill1Name.text = $"{EPlayerSkillType.TimeFreeze}";
        _skill2Name.text = $"{EPlayerSkillType.NaturalDisaster}";
        _skillImage[0].sprite = _skillImageSprites[0];
        _skillImage[1].sprite = _skillImageSprites[1];
    }

    private void Esc()
    {
        gameObject.SetActive(false);
    }
}
