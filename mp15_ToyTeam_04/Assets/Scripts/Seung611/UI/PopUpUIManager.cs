using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _hp;
    [SerializeField] private GameObject _battle;
    [SerializeField] private GameObject _towerSpecPopUp;
    [SerializeField] private GameObject _playerSkillPopUp;
    [SerializeField] private GameObject _stageResultPopUp;
    [SerializeField] private GameObject _messagePopUp;
    [SerializeField] private GameObject _setting;
    [SerializeField] private GameObject _towerSelectTilePopUp;

    private void Awake() => Init();
    private void OnEnable() => BindEventButtons();
    private void OnDisable() => UnbindEventButtons();
    
    private void BindEventButtons()
    {
        PopUpManager.Instance.OnGameSettingsPopUp += OnSetting;
        PopUpManager.Instance.OnGamePlayerSkillPopUp += OnPlayerSkillPopUp;
        PopUpManager.Instance.OnGameMessagePopUp += OnMessagePopUp;
        PopUpManager.Instance.OnGameStageResultPopUp += OnStageResultPopUp;
        PopUpManager.Instance.OnGameTowerSpecPopUp += OnTowerSpecPopUp;
        PopUpManager.Instance.OnGameTowerSelectTilePopup += OnTowerSelectTilePopUp;
    }

    private void UnbindEventButtons()
    {
        PopUpManager.Instance.OnGameSettingsPopUp -= OnSetting;
        PopUpManager.Instance.OnGamePlayerSkillPopUp -= OnPlayerSkillPopUp;
        PopUpManager.Instance.OnGameMessagePopUp -= OnMessagePopUp;
        PopUpManager.Instance.OnGameStageResultPopUp -= OnStageResultPopUp;
        PopUpManager.Instance.OnGameTowerSpecPopUp -= OnTowerSpecPopUp;
        PopUpManager.Instance.OnGameTowerSelectTilePopup -= OnTowerSelectTilePopUp;
    }
    
    private void OnSetting()
    {
        _setting.SetActive(true);
    }

    private void OnPlayerSkillPopUp()
    {
        _playerSkillPopUp.SetActive(true);
    }

    private void OnMessagePopUp()
    {
        _messagePopUp.SetActive(true);
    }

    private void OnStageResultPopUp()
    {
        _stageResultPopUp.SetActive(true);
    }

    private void OnTowerSpecPopUp()
    {
        _towerSpecPopUp.SetActive(true);
    }

    private void OnTowerSelectTilePopUp()
    {
        _towerSelectTilePopUp.SetActive(true);
    }
    
    
    private void Init()
    {
        _hp.SetActive(false);
        _battle.SetActive(false);
        _towerSpecPopUp.SetActive(false);
        _playerSkillPopUp.SetActive(false);
        _stageResultPopUp.SetActive(false);
        _messagePopUp.SetActive(false);
        _setting.SetActive(false);
        _towerSelectTilePopUp.SetActive(false);
    }
}
