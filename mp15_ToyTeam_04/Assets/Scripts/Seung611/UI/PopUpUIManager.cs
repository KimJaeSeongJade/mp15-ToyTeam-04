using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpUIManager : MonoBehaviour
{
    private void Awake() => Init();
    private void OnEnable() => BindEventButtons();
    private void OnDisable() => UnbindEventButtons();
    
    private void BindEventButtons()
    {
        UIManager.Instance.PopUp.OnGameSettingsPopUp += OnSetting;
        UIManager.Instance.PopUp.OnGamePlayerSkillPopUp += OnPlayerSkillPopUp;
        UIManager.Instance.PopUp.OnGameMessagePopUp += OnMessagePopUp;
        UIManager.Instance.PopUp.OnGameStageResultPopUp += OnStageResultPopUp;
        UIManager.Instance.PopUp.OnGameTowerSpecPopUp += OnTowerSpecPopUp;
        UIManager.Instance.PopUp.OnGameTowerSelectTilePopup += OnTowerSelectTilePopUp;
    }

    private void UnbindEventButtons()
    {
        UIManager.Instance.PopUp.OnGameSettingsPopUp -= OnSetting;
        UIManager.Instance.PopUp.OnGamePlayerSkillPopUp -= OnPlayerSkillPopUp;
        UIManager.Instance.PopUp.OnGameMessagePopUp -= OnMessagePopUp;
        UIManager.Instance.PopUp.OnGameStageResultPopUp -= OnStageResultPopUp;
        UIManager.Instance.PopUp.OnGameTowerSpecPopUp -= OnTowerSpecPopUp;
        UIManager.Instance.PopUp.OnGameTowerSelectTilePopup -= OnTowerSelectTilePopUp;
    }
    
    private void OnSetting()
    {
        UIManager.Instance.PopUp.SettingPopUp.gameObject.SetActive(true);
    }

    private void OnPlayerSkillPopUp()
    {
        UIManager.Instance.PopUp.PlayerSkillPopUp.gameObject.SetActive(true);
    }

    private void OnMessagePopUp()
    {
        UIManager.Instance.PopUp.MessagePopUp.gameObject.SetActive(true);
    }

    private void OnStageResultPopUp()
    {
        UIManager.Instance.PopUp.StageResultPopUp.gameObject.SetActive(true);
    }

    private void OnTowerSpecPopUp()
    {
        UIManager.Instance.PopUp.TowerSpecPopUp.gameObject.SetActive(true);
    }

    private void OnTowerSelectTilePopUp()
    {
        UIManager.Instance.PopUp.TowerSelectTilePopUp.gameObject.SetActive(true);
    }
    
    
    private void Init()
    {
        UIManager.Instance.PopUp.Hp.gameObject.SetActive(false);
        UIManager.Instance.PopUp.SettingPopUp.gameObject.SetActive(false);
        UIManager.Instance.PopUp.PlayerSkillPopUp.gameObject.SetActive(false);
        UIManager.Instance.PopUp.MessagePopUp.gameObject.SetActive(true);
        UIManager.Instance.PopUp.StageResultPopUp.gameObject.SetActive(false);
        UIManager.Instance.PopUp.TowerSpecPopUp.gameObject.SetActive(false);
        UIManager.Instance.PopUp.TowerSelectTilePopUp.gameObject.SetActive(false);
    }
}
