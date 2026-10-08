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
    [SerializeField] private TextMeshProUGUI _monsterCount;

    private void Awake() => Init();
    private void OnEnable() => BindButtonEvents();
    private void OnDisable() => UnbindButtonEvents();
    
    // Start 버튼 눌렀을 때 Battle 화면으로
    private void BindButtonEvents()
    {
        _playerSkillButton.onClick.AddListener(SkillPopUp);
        _settingButton.onClick.AddListener(SettingPopUp);
        _towerInventory[0].onClick.AddListener(Tower0Select);
        _towerInventory[1].onClick.AddListener(Tower1Select);
        _towerInventory[2].onClick.AddListener(Tower2Select);
    }
    
    private void UnbindButtonEvents()
    {
        _playerSkillButton.onClick.RemoveListener(SkillPopUp);
        _settingButton.onClick.RemoveListener(SettingPopUp);
        _towerInventory[0].onClick.RemoveListener(Tower0Select);
        _towerInventory[1].onClick.RemoveListener(Tower1Select);
        _towerInventory[2].onClick.RemoveListener(Tower2Select);
    }

    private void SkillPopUp()
    {
        // 스킬을 사용 쿨타임 돌기
    }

    private void SettingPopUp()
    {
        UIManager.Instance.PopUp.SettingPopUpOpen();
    }

    public void PushInstallButton()
    {
        _towerInventory[0].gameObject.SetActive(true);
        _towerInventory[1].gameObject.SetActive(true);
        _towerInventory[2].gameObject.SetActive(true);
    }

    private void Tower0Select()
    {
        // ArrowTower 설치
        PlayerManager.Instance.InstallTower(ETowerType.ArrowTower);
        InstallComplete();
    }


    private void Tower1Select()
    {
        // FireTower 설치
        PlayerManager.Instance.InstallTower(ETowerType.FireTower);
        InstallComplete();
    }

    private void Tower2Select()
    {
        // IceTower 설치
        PlayerManager.Instance.InstallTower(ETowerType.IceTower);
        InstallComplete();
    }
    private void InstallComplete()
    {
        UIManager.Instance.PopUp.TowerSelectTilePopUp.CancelButton();
        UIManager.Instance.Window.BattleWindow.HideInstallButton();
    }

    private void Init()
    {
        HideInstallButton();
    }

    public void HideInstallButton()
    {
        _towerInventory[0].gameObject.SetActive(false);
        _towerInventory[1].gameObject.SetActive(false);
        _towerInventory[2].gameObject.SetActive(false);
    }

    public void CurrnetMonster()
    {
        _monsterCount.text = _monsterCount.ToString() + " / " + _monsterCount.ToString();
    }

    public void HaveGold(int gold)
    {
        _haveGlod.text = "Gold: " + gold.ToString();
    }
}