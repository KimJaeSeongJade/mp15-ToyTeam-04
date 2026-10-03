using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    private void OnGameBattle()
    {
        _battle.SetActive(false);
    }
    

    private void Init()
    {
        _lobby.SetActive(true);
        _battle.SetActive(false);
    }
}
