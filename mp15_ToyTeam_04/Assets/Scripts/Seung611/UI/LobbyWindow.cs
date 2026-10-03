using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyWindow : MonoBehaviour
{
    [SerializeField] private Image _characterProfile;
    [SerializeField] private Button _gameStartButton;
    [SerializeField] private Button _playerSkillButton;
    [SerializeField] private Button _settingButton;
    [SerializeField] private List<Button> _towerInventory;
    public int _hasGold;
    
    private void OnEnable() => BindButtonEvents();
    private void OnDisable() => UnbindButtonEvents();
    
    // Start 버튼 눌렀을 때 Battle 화면으로
    private void BindButtonEvents()
    {
        _gameStartButton.onClick.AddListener(LoadGameScene);
    }
    
    private void UnbindButtonEvents()
    {
        _gameStartButton.onClick.RemoveListener(LoadGameScene);
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene(2);
    }
    
}
