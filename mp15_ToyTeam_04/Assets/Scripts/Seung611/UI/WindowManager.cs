using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    public TitleWindow TitleWindow;
    public LobbyWindow LobbyWindow;
    public BattleWindow BattleWindow;

    public Loading LoadingWindow;

    public GameObject TimeEffect;

    // public BattleSub _battleSub;

    public event Action OnGameTitle;
    public event Action OnGameLobby;
    public event Action OnGameBattle;

    public void Start()
    {
        SoundManager.Instance.PlayBgm(EBgm.TITLE);
    }

    public void TitleWindowOpen()
    {
        OnGameTitle?.Invoke();
        SoundManager.Instance.PlayBgm(EBgm.TITLE);
    }
    
    public void LobbyWindowOpen()
    {
        TimeEffect.SetActive(false);
        PlayerManager.Instance?.TimeStopEffect.SetActive(false);
        OnGameLobby?.Invoke();
        SoundManager.Instance.PlayBgm(EBgm.LOBBY);
    }

    public void BattleWindowOpen()
    {
        OnGameBattle?.Invoke();
        SoundManager.Instance.PlayBgm(EBgm.BATTLE);
    }

    public void LoadWindowOpen()
    {
        LoadingWindow.gameObject.SetActive(true);
        LoadingWindow.LoadingStart();
    }

    public void TimeEffectOpen()
    {
        TimeEffect.gameObject.SetActive(true);
        StartCoroutine(TimeStop());
    }

    private IEnumerator TimeStop()
    {
        yield return new WaitForSeconds(10f);
        TimeEffect.gameObject.SetActive(false);
    }
}