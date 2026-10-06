using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class LobbyWindow : MonoBehaviour
{
    [SerializeField] private Button _gameStartButton;
    [SerializeField] private Button _playerSkillButton;
    [SerializeField] private Button _settingButton;
    [SerializeField] private List<Button> _towerInventory;
    [SerializeField] private TextMeshProUGUI _haveGlod;
    [SerializeField] private TowerSpecPopUp _towerSpecPopUp;
    TowerState[] _towerState = new TowerState[3];
    // 스테이지에서 값 가져오기    
    public int _haveGoldCount;
    
    private void Update() => HaveGold();
    private void OnEnable() => BindButtonEvents();
    private void OnDisable() => UnbindButtonEvents();
    
    // Start 버튼 눌렀을 때 Battle 화면으로
    private void BindButtonEvents()
    {
        _gameStartButton.onClick.AddListener(StartGame);
        _playerSkillButton.onClick.AddListener(SkillPopUp);
        _settingButton.onClick.AddListener(SettingPopUp);
        _towerInventory[0].onClick.AddListener(Tower0SpecPopUp);
        _towerInventory[1].onClick.AddListener(Tower1SpecPopUp);
        _towerInventory[2].onClick.AddListener(Tower2SpecPopUp);
    }
    
    private void UnbindButtonEvents()
    {
        _gameStartButton.onClick.RemoveListener(StartGame);
        _playerSkillButton.onClick.RemoveListener(SkillPopUp);
        _settingButton.onClick.RemoveListener(SettingPopUp);
        _towerInventory[0].onClick.RemoveListener(Tower0SpecPopUp);
        _towerInventory[1].onClick.RemoveListener(Tower1SpecPopUp);
        _towerInventory[2].onClick.RemoveListener(Tower2SpecPopUp);
    }

    private void StartGame()
    {
        UIManager.Instance.Window.BattleWindowOpen();
    }

    private void SkillPopUp()
    {
        UIManager.Instance.PopUp.PLayerSkillPopUpOpen();
        
    }

    private void SettingPopUp()
    {
        UIManager.Instance.PopUp.SettingPopUpOpen();
    }

    private void Tower0SpecPopUp()
    {
        _towerState[0] = new ArrowTower();
        UIManager.Instance.PopUp.TowerSpecPopUp.SetData(_towerState, 0);
        UIManager.Instance.PopUp.TowerSpecPopUpOpen(0); // 팝업 오픈 용도
    }

    private void Tower1SpecPopUp()
    {
        _towerState[1] = new FireTower();
        UIManager.Instance.PopUp.TowerSpecPopUp.SetData(_towerState, 1);
        UIManager.Instance.PopUp.TowerSpecPopUpOpen(1);
    }

    private void Tower2SpecPopUp()
    {
        _towerState[2] = new IceTower();
        UIManager.Instance.PopUp.TowerSpecPopUp.SetData(_towerState, 2);
        UIManager.Instance.PopUp.TowerSpecPopUpOpen(2);
    }

    private void HaveGold()
    {
        _haveGlod.text = "Gold: " + _haveGoldCount.ToString();
    }
}
