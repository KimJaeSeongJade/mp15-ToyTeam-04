using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleWindow : MonoBehaviour
{
    [SerializeField] private Button _gameStartButton;
    [SerializeField] private Button _settingButton;
    // [SerializeField] private TextMeshPro _titleText;
    
    private void OnEnable() => BindButtonEvents();
    private void OnDisable() => UnbindButtonEvents();

    private void BindButtonEvents()
    {
        _gameStartButton.onClick.AddListener(LobbyWindow);
        _settingButton.onClick.AddListener(SettingPopUp);
    }

    private void UnbindButtonEvents()
    {
        _gameStartButton.onClick.RemoveListener(LobbyWindow);
        _settingButton.onClick.RemoveListener(SettingPopUp);
    }

    private void LobbyWindow()
    {
        UIManager.Instance.Window.LobbyWindowOpen();
        SceneManager.LoadScene(2);
    }

    private void SettingPopUp()
    {
        UIManager.Instance.PopUp.SettingPopUpOpen();
    }
}
