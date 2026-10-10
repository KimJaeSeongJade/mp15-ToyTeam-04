using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillPopUp : MonoBehaviour
{
    [SerializeField] private Button _skill1SelectButton;
    [SerializeField] private Button _skill2SelectButton;
    [SerializeField] private List<TextMeshProUGUI> _skillName;
    [SerializeField] private List<Image> _skillImage;
    [SerializeField] private Button _escButton;
    
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
    
    public void SetData(EPlayerSkillType type, Sprite sprite, int index)
    {
        _skillName[index].text = $"{type}";
        // 공격 특성 추가하면 좋을 듯
        _skillImage[index].sprite = sprite;
    }
    
    private void Skill1Selected()
    {
        // Skill1을 눌렀을 때 어떤 동작을 할 지 구현
        SoundManager.Instance.PlaySfx(ESfx.BUTTON_CLICK);
        gameObject.SetActive(false);
        UIManager.Instance.Window.LobbyWindow.SetData(_skillImage[0].sprite);
        PlayerManager.Instance.EquipSkill(EPlayerSkillType.TimeFreeze);
    }
    
    private void Skill2Selected()
    {
        // Skill2를 눌렀을 때 어떤 동작을 할 지 구현
        SoundManager.Instance.PlaySfx(ESfx.BUTTON_CLICK);
        gameObject.SetActive(false);
        UIManager.Instance.Window.LobbyWindow.SetData(_skillImage[1].sprite);
        PlayerManager.Instance.EquipSkill(EPlayerSkillType.NaturalDisaster);
    }

    private void Esc()
    {
        SoundManager.Instance.PlaySfx(ESfx.BUTTON_CLICK);
        gameObject.SetActive(false);
    }
}
