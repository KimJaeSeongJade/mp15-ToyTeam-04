using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    public TowerSpecPopUp TowerSpecPopUp;
    public PlayerSkillPopUp PlayerSkillPopUp;
    public StageResultPopUp StageResultPopUp;
    public MessagePopUp MessagePopUp;
    public SettingPopUp SettingPopUp;
    public TowerSelectTilePopUp TowerSelectTilePopUp;
    
    public event Action OnGameSettingsPopUp;
    public event Action OnGameStageResultPopUp;
    public event Action OnGamePlayerSkillPopUp;
    public event Action OnGameTowerSpecPopUp;
    public event Action<bool> OnGameTowerSelectTilePopup;
    public event Action OnGameMessagePopUp;

    public void SettingPopUpOpen()
    {
        OnGameSettingsPopUp?.Invoke();
    }

    public void StageResultPopUpOpen()
    {
        OnGameStageResultPopUp?.Invoke();
    }
    
    public void PLayerSkillPopUpOpen()
    {
        OnGamePlayerSkillPopUp?.Invoke();
    }

    public void TowerSpecPopUpOpen()
    {
        OnGameTowerSpecPopUp?.Invoke();
    }

    public void TowerSelectTilePopUpOpen(bool isTower)
    {
        OnGameTowerSelectTilePopup?.Invoke(isTower);
    }

    public void MessagePopUpOpen()
    {
        OnGameMessagePopUp?.Invoke();
    }
}