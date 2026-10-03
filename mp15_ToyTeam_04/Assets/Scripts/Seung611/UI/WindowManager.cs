using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : Singleton<WindowManager>
{
    private TitleWindow _titleWindow;
    private LobbyWindow _lobbyWindow;
    private BattleWindow _battleWindow;
    private BattleSub _battleSub;
    
    public event Action OnGameLobby;
    public event Action OnGameBattle;

    private void Awake()
    {
        SetSingleton();
    }

    public void SetLobbyWindow()
    {
        OnGameLobby?.Invoke();
    }

    public void SetBattleWindow()
    {
        OnGameBattle?.Invoke();
    }
}