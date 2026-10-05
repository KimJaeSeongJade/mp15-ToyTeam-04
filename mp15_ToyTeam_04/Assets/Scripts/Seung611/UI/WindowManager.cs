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

    public void LobbyWindow()
    {
        OnGameLobby?.Invoke();
        SoundManager.Instance.PlayBgm(EBgm.LOBBY);
    }

    public void BattleWindow()
    {
        OnGameBattle?.Invoke();
        SoundManager.Instance.PlayBgm(EBgm.GAME);
    }
}