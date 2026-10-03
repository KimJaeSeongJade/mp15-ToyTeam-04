using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleWindow : MonoBehaviour
{
    [SerializeField] private Image _backGroundScene;
    [SerializeField] private Button _gameStartButton;
    [SerializeField] private Button _settingButton;
    // [SerializeField] private TextMeshPro _titleText;
    
    private void OnEnable() => BindButtonEvents();
    private void OnDisable() => UnbindButtonEvents();

    private void BindButtonEvents()
    {
        _gameStartButton.onClick.AddListener(LoadLobbyScene);
    }

    private void UnbindButtonEvents()
    {
        _gameStartButton.onClick.RemoveListener(LoadLobbyScene);
    }

    private void LoadLobbyScene()
    {
        SceneManager.LoadScene(1);
    }
}
