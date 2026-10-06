using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public WindowManager Window;
    public PopUpManager PopUp;

    private void Awake()
    {
        SetSingleton();
        CacheComponents();
    }

    private void CacheComponents()
    {
        Window = GetComponentInChildren<WindowManager>();
        PopUp = GetComponentInChildren<PopUpManager>();
    }
}