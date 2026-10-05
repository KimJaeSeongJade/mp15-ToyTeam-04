using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WindowUIManager : MonoBehaviour
{
    private void Awake() => Init();
    private void OnEnable() => BindEventButtons();
    private void OnDisable() => UnbindEventButtons();
    
    private void BindEventButtons()
    {
        UIManager.Instance.Window.OnGameTitle += OnGameTitle;
        UIManager.Instance.Window.OnGameLobby += OnGameLobby;
        UIManager.Instance.Window.OnGameBattle += OnGameBattle;
    }
    
    private void UnbindEventButtons()
    {
        UIManager.Instance.Window.OnGameTitle -= OnGameTitle;
        UIManager.Instance.Window.OnGameLobby -= OnGameLobby;
        UIManager.Instance.Window.OnGameBattle -= OnGameBattle;
    }

    private void OnGameTitle()
    {
        UIManager.Instance.Window.TitleWindow.gameObject.SetActive(true);
        UIManager.Instance.Window.LobbyWindow.gameObject.SetActive(false);
        UIManager.Instance.Window.BattleWindow.gameObject.SetActive(false);
    }

    private void OnGameLobby()
    {
        UIManager.Instance.Window.TitleWindow.gameObject.SetActive(false);
        UIManager.Instance.Window.LobbyWindow.gameObject.SetActive(true);
        UIManager.Instance.Window.BattleWindow.gameObject.SetActive(false);
    }

    private void OnGameBattle()
    {
        UIManager.Instance.Window.TitleWindow.gameObject.SetActive(false);
        UIManager.Instance.Window.LobbyWindow.gameObject.SetActive(false);
        UIManager.Instance.Window.BattleWindow.gameObject.SetActive(true);
    }
    

    private void Init()
    {
        OnGameTitle();
    }
}
