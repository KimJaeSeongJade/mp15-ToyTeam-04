using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class LobbyWindow : MonoBehaviour
{
    [SerializeField] private Button _gameStartButton;
    [SerializeField] private Button _settingButton;
    [SerializeField] private List<Button> _towerInventory;
    [SerializeField] private TextMeshProUGUI _haveGlod;
    [SerializeField] private Button _playerSkillButton;
    [SerializeField] private Image _playerSkillImage;
    [SerializeField] private List<Sprite> _skillImageSprites;
    private TowerState[] _towerState = new TowerState[3];

    private void OnEnable() => BindButtonEvents();
    private void OnDisable() => UnbindButtonEvents();
    private void Update() => Escape();
    
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
    
    private void Escape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance.PopUp.SettingPopUpOpen();
        }
    }

    public void SetData(Sprite sprite)
    {
        _playerSkillImage.sprite = sprite;
    }

    private void StartGame()
    {
        UIManager.Instance.Window.BattleWindow.Init();
        UIManager.Instance.Window.BattleWindow.SetData(_playerSkillImage.sprite);
        UIManager.Instance.Window.BattleWindowOpen();
        MapManager.Instance.ShowBattleMap();
    }

    private void SkillPopUp()
    {
        UIManager.Instance.PopUp.PlayerSkillPopUp.SetData((EPlayerSkillType)0, _skillImageSprites[0], 0);
        UIManager.Instance.PopUp.PlayerSkillPopUp.SetData((EPlayerSkillType)1, _skillImageSprites[1], 1);
        UIManager.Instance.PopUp.PLayerSkillPopUpOpen();
    }

    private void SettingPopUp()
    {
        UIManager.Instance.PopUp.SettingPopUpOpen();
    }
    
    private void Tower0SpecPopUp()
    {
        _towerState[0] = PlayerManager.Instance.GetTowerState(ETowerType.ArrowTower);
        UIManager.Instance.PopUp.TowerSpecPopUp.SetData(_towerState, 0);
        UIManager.Instance.PopUp.TowerSpecPopUpOpen();
    }

    private void Tower1SpecPopUp()
    {
        _towerState[1] = PlayerManager.Instance.GetTowerState(ETowerType.FireTower);
        UIManager.Instance.PopUp.TowerSpecPopUp.SetData(_towerState, 1);
        UIManager.Instance.PopUp.TowerSpecPopUpOpen();
    }

    private void Tower2SpecPopUp()
    {
        _towerState[2] = PlayerManager.Instance.GetTowerState(ETowerType.IceTower);
        UIManager.Instance.PopUp.TowerSpecPopUp.SetData(_towerState, 2);
        UIManager.Instance.PopUp.TowerSpecPopUpOpen();
    }

    public void HaveGold(int gold)
    {
        _haveGlod.text = gold.ToString();
    }
}
