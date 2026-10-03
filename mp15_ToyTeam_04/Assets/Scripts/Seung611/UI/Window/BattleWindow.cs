using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleWindow : MonoBehaviour
{
    [SerializeField] private Button _playerSkillButton;
    [SerializeField] private Button _settingButton;
    [SerializeField] private List<Button> _towerInventory;
    [SerializeField] private TextMeshProUGUI _haveGlod;
    // 스테이지에서 값 가져오기
    public int _haveGoldCount;

    private void Start() => gameObject.SetActive(true);
    private void Update() => HaveGlod();
    private void OnEnable() => BindButtonEvents();
    private void OnDisable() => UnbindButtonEvents();
    
    // Start 버튼 눌렀을 때 Battle 화면으로
    private void BindButtonEvents()
    {
        _playerSkillButton.onClick.AddListener(SkillPopUp);
        _settingButton.onClick.AddListener(SettingPopUp);
        _towerInventory[0].onClick.AddListener(TowerSpecPopUp);
    }
    
    private void UnbindButtonEvents()
    {
        _playerSkillButton.onClick.RemoveListener(SkillPopUp);
        _settingButton.onClick.RemoveListener(SettingPopUp);
        _towerInventory[0].onClick.RemoveListener(TowerSpecPopUp);
    }

    private void SkillPopUp()
    {
        PopUpManager.Instance.PLayerSkillPopUp();
    }

    private void SettingPopUp()
    {
        PopUpManager.Instance.SettingPopUp();
    }

    private void TowerSpecPopUp()
    {
        PopUpManager.Instance.TowerSpecPopUp();
    }

    private void HaveGlod()
    {
        _haveGlod.text = "Glod: " + _haveGoldCount.ToString();
    }
}