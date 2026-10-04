using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WindowUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _lobby;
    [SerializeField] private GameObject _battle;
    
    private void Awake() => Init();
    private void OnEnable() => BindEventButtons();
    private void OnDisable() => UnbindEventButtons();
    
    private void BindEventButtons()
    {
        WindowManager.Instance.OnGameLobby += OnGameLobby;
        WindowManager.Instance.OnGameBattle += OnGameBattle;
    }
    
    private void UnbindEventButtons()
    {
        WindowManager.Instance.OnGameLobby -= OnGameLobby;
        WindowManager.Instance.OnGameBattle -= OnGameBattle;
    }

    private void OnGameLobby()
    {
        _lobby.SetActive(true);
        _battle.SetActive(false);
    }

    private void OnGameBattle()
    {
        _battle.SetActive(true);
    }
    

    private void Init()
    {
        if (SceneManager.GetActiveScene().name == "Seung611_UI")
        {
            _lobby.SetActive(true);
            _battle.SetActive(false);
        }
    }
}
