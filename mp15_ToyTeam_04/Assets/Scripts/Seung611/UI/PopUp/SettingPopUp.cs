using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingPopUp : MonoBehaviour
{
    [SerializeField] private Button _sfxSoundButton;
    [SerializeField] private Button _bgmSoundButton;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _lobbyButton;


    [SerializeField] private Animator _bgmAni;
    [SerializeField] private Animator _sfxAni;

    [SerializeField] private TextMeshProUGUI _bgmText;
    [SerializeField] private TextMeshProUGUI _sfxText;

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
        _sfxSoundButton.onClick.RemoveListener(SFXSound);
        _bgmSoundButton.onClick.RemoveListener(BGMSound);
        _continueButton.onClick.RemoveListener(Continue);
        _lobbyButton.onClick.RemoveListener(LobbyWindow);
    }

    private void SFXSound()
    {
        SoundManager.Instance.PlayStopSfx(!SoundManager.Instance.IsSfx);
        if (!SoundManager.Instance.IsSfx)
        {
            _sfxAni.SetTrigger("SFX_OFF");
            _sfxText.text = "OFF";
        }
        else
        {
            _sfxAni.SetTrigger("SFX_ON");
            _sfxText.text = "ON";
        }
    }

    private void BGMSound()
    {
        SoundManager.Instance.PlayStopBgm(!SoundManager.Instance.IsBgm);
        if (!SoundManager.Instance.IsBgm)
        {
            _bgmAni.SetTrigger("BGM_OFF");
            _bgmText.text = "OFF";
        }
        else
        {
            _bgmAni.SetTrigger("BGM_ON");
            _bgmText.text = "ON";
        }
    }

    private void Continue()
    {
        SoundManager.Instance.PlaySfx(ESfx.BUTTON_CLICK);
        UIManager.Instance.PopUp.SettingPopUp.gameObject.SetActive(false);
        PlayerManager.Instance.SetUIMode(false);
    }

    private void LobbyWindow()
    {
        // LobbyWindow 띄우기
        SoundManager.Instance.PlaySfx(ESfx.BUTTON_CLICK);
        UIManager.Instance.PopUp.SettingPopUp.gameObject.SetActive(false);
        UIManager.Instance.Window.LobbyWindowOpen();
        MapManager.Instance.ResetMap();
        StageManager.Instance.StageFail();
        PlayerManager.Instance.OffPlayer();
    }
}
