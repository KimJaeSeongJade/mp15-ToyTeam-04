using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpManager : Singleton<PopUpManager>
{
    public event Action OnGameSettingsPopUp;
    public event Action OnGameStageResultPopUp;
    public event Action OnGamePlayerSkillPopUp;
    public event Action OnGameTowerSpecPopUp;
    public event Action OnGameTowerSelectTilePopup;
    public event Action OnGameMessagePopUp;


    private void Awake() => SetSingleton();
    

    public void SettingPopUp()
    {
        OnGameSettingsPopUp?.Invoke();
    }

    public void StageResultPopUp()
    {
        OnGameStageResultPopUp?.Invoke();
    }
    
    public void PLayerSkillPopUp()
    {
        OnGamePlayerSkillPopUp?.Invoke();
    }

    public void TowerSpecPopUp()
    {
        OnGameTowerSpecPopUp?.Invoke();
    }

    public void TowerSelectTilePopUp()
    {
        OnGameTowerSelectTilePopup?.Invoke();
    }

    public void MessagePopUp()
    {
        OnGameMessagePopUp?.Invoke();
    }

}