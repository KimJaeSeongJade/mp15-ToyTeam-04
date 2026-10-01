using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private WindowManager _windowManager;
    private PopUpManager _popUpManager;
    
    private void Awake() => CacheComponents();

    private void CacheComponents()
    {
        _windowManager = GetComponent<WindowManager>();
        _popUpManager = GetComponent<PopUpManager>();
    }
}