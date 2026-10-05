using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    public TitleWindow TitleWindow;
    public LobbyWindow LobbyWindow;
    public BattleWindow BattleWindow;
    // public BattleSub _battleSub;

    public event Action OnGameTitle;
    public event Action OnGameLobby;
    public event Action OnGameBattle;

    public void TitleWindowOpen()
    {
        OnGameTitle?.Invoke();
        SoundManager.Instance.PlayBgm(EBgm.TITLE);
    }
    
    public void LobbyWindowOpen()
    {
        OnGameLobby?.Invoke();
        SoundManager.Instance.PlayBgm(EBgm.LOBBY);
    }

    public void BattleWindowOpen()
    {
        OnGameBattle?.Invoke();
        SoundManager.Instance.PlayBgm(EBgm.GAME);
    }
}