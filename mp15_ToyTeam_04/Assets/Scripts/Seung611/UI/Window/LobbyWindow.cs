using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyWindow : MonoBehaviour
{
    [SerializeField] private Button _gameStartButton;
    [SerializeField] private Button _playerSkillButton;
    [SerializeField] private Button _settingButton;
    [SerializeField] private List<Button> _towerInventory;
    [SerializeField] private TextMeshProUGUI _haveGlod;
    [SerializeField] private TowerSpecPopUp _towerSpecPopUp;


    private void SetData(int Value)
    {
        // 텍스트 설정함
    }
    
    private Dictionary<Button, int> _towerButton;
    // 스테이지에서 값 가져오기    
    public int _haveGoldCount;

    public event Action<int> OnTowerButton;
    
    private void Update() => HaveGlod();
    private void OnEnable() => BindButtonEvents();
    private void OnDisable() => UnbindButtonEvents();
    
    // Start 버튼 눌렀을 때 Battle 화면으로
    private void BindButtonEvents()
    {
        _gameStartButton.onClick.AddListener(StartGame);
        _playerSkillButton.onClick.AddListener(SkillPopUp);
        _settingButton.onClick.AddListener(SettingPopUp);
        _towerInventory[0].onClick.AddListener(TowerSpecPopUp);
        _towerInventory[1].onClick.AddListener(TowerSpecPopUp);
        _towerInventory[2].onClick.AddListener(TowerSpecPopUp);
    }
    
    private void UnbindButtonEvents()
    {
        _gameStartButton.onClick.RemoveListener(StartGame);
        _playerSkillButton.onClick.RemoveListener(SkillPopUp);
        _settingButton.onClick.RemoveListener(SettingPopUp);
        _towerInventory[0].onClick.RemoveListener(TowerSpecPopUp);
        _towerInventory[1].onClick.RemoveListener(TowerSpecPopUp);
        _towerInventory[2].onClick.RemoveListener(TowerSpecPopUp);
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

    /*public void TowerSpecPopUp()
    {
        // OnTowerButton?.Invoke();
    }*/

    private void TowerSpecPopUp()
    {
        if (_towerInventory[0])
        {
            OnTowerButton?.Invoke(0);
        }

        if (_towerInventory[1])
        {
            OnTowerButton?.Invoke(1);
        }

        if (_towerInventory[2])
        {
            OnTowerButton?.Invoke(2);
        }
    }

    private void HaveGlod()
    {
        _haveGlod.text = "Glod: " + _haveGoldCount.ToString();
    }
}
