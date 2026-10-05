using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingPopUp : MonoBehaviour
{
    [SerializeField] private Button _sfxSoundButton;
    [SerializeField] private Button _bgmSoundButton;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _lobbyButton;
    
    private void OnEnable() => BindButtonEvents();
    
    private void OnDisable() => UnbindButtonEvents();

    private void BindButtonEvents()
    {
        _sfxSoundButton.onClick.AddListener(SFXSound);
        _bgmSoundButton.onClick.AddListener(BGMSound);
        _continueButton.onClick.AddListener(Continue);
        _lobbyButton.onClick.AddListener(LobbyWindow);
    }
    
    private void UnbindButtonEvents()
    {
        _sfxSoundButton.onClick.AddListener(SFXSound);
        _bgmSoundButton.onClick.AddListener(BGMSound);
        _continueButton.onClick.AddListener(Continue);
        _lobbyButton.onClick.AddListener(LobbyWindow);
    }

    private void SFXSound()
    {
        
    }

    private void BGMSound()
    {
        
    }

    private void Continue()
    {
        UIManager.Instance.PopUp.SettingPopUp.gameObject.SetActive(false);
    }

    private void LobbyWindow()
    {
        // LobbyWindow 띄우기
        // WindowManager.Instance.LobbyWindow();
        SceneManager.LoadScene(2);
        UIManager.Instance.PopUp.SettingPopUp.gameObject.SetActive(false);
        UIManager.Instance.Window.LobbyWindowOpen();
    }
}
