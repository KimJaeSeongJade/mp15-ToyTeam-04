using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour, IWindowable
{
    private TitleWindow _titleWindow;
    private LobbyWindow _lobbyWindow;
    private BattleWindow _battleWindow;
    private BattleSub _battleSub;

    public void Init()
    {
        
    }

    public void ScreenRefresh()
    {
        
    }
}