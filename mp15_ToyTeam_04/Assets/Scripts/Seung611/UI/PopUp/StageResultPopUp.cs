using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageResultPopUp : MonoBehaviour
{
    [SerializeField] private Button _nextStage;
    [SerializeField] private Button _lobby;
    [SerializeField] private TextMeshProUGUI _stageNumber;
    [SerializeField] private TextMeshProUGUI _waveNumber;
    [SerializeField] private TextMeshProUGUI _monsterCount;
    [SerializeField] private TextMeshProUGUI _getGold;
    [SerializeField] private TextMeshProUGUI _stageReward;
    
    // 스테이지에서 값 받아오기 (추후에 값 수정)
    private int _stageCount => StageManager.Instance.StageNumber; 
    private int _waveCount => StageManager.Instance.WaveNumber; 
    private int _currentMonsterCount => StageManager.Instance.monsterNumber; // 클리어한 몬스터 수 받기
    private int _maxMonsterCount => StageManager.Instance.totalMonsterNumber;
    private int _getGoldCount => StageManager.Instance._stageEarnedGold;
    private int _stageRewardCount => StageManager.Instance.StageClearReward;

    private void OnEnable() => BindButtonEvents();
    private void OnDisable() => UnbindButtonEvents();

    private void BindButtonEvents()
    {
        _nextStage.onClick.AddListener(NextStage);
        _lobby.onClick.AddListener(Lobby);
    }
    
    private void UnbindButtonEvents()
    {
        _nextStage.onClick.RemoveListener(NextStage);
        _lobby.onClick.RemoveListener(Lobby);
    }

    private void NextStage()
    {
        SoundManager.Instance.PlaySfx(ESfx.BUTTON_CLICK);
        gameObject.SetActive(false);
        MapManager.Instance.ShowBattleMap();
        PlayerManager.Instance.SetUIMode(false);
    }

    private void Lobby()
    {
        SoundManager.Instance.PlaySfx(ESfx.BUTTON_CLICK);
        gameObject.SetActive(false);
        UIManager.Instance.Window.LobbyWindowOpen();
    }

    public void Result()
    {
        _stageNumber.text = $"Stage # " + _stageCount.ToString();
        _waveNumber.text = $"Wave # " + _waveCount.ToString();
        _monsterCount.text = _currentMonsterCount.ToString() + " / " + _maxMonsterCount.ToString();
        _getGold.text = $"Gold : " + _getGoldCount.ToString();
        _stageReward.text = $"Reward : " + _stageRewardCount.ToString();
    }
}
