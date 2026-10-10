using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPopUp : MonoBehaviour
{
    [SerializeField] private Button _again;
    [SerializeField] private Button _lobby;
    [SerializeField] private TextMeshProUGUI _stageNumber;
    [SerializeField] private TextMeshProUGUI _waveNumber;
    
    private int _stageCount => StageManager.Instance.StageNumber; 
    private int _waveCount => StageManager.Instance.WaveNumber; 
    
    private void OnEnable() => BindButtonEvents();
    private void OnDisable() => UnbindButtonEvents();

    private void BindButtonEvents()
    {
        _again.onClick.AddListener(Again);
        _lobby.onClick.AddListener(Lobby);
    }
    
    private void UnbindButtonEvents()
    {
        _again.onClick.RemoveListener(Again);
        _lobby.onClick.RemoveListener(Lobby);
    }

    private void Again()
    {
        gameObject.SetActive(false);
        PlayerManager.Instance.OnPlayer();
        UIManager.Instance.Window.BattleWindow.Init();
        UIManager.Instance.Window.BattleWindowOpen();
        MapManager.Instance.ShowBattleMap();
    }

    private void Lobby()
    {
        gameObject.SetActive(false);
        UIManager.Instance.Window.LobbyWindowOpen();
    }

    public void Result()
    {
        _stageNumber.text = $"Stage # " + _stageCount.ToString();
        _waveNumber.text = $"Wave # " + _waveCount.ToString();
    }
}
