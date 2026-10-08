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

    private StageManager _stage;
    private StageManager.MonsterStatUp _monsterStat;
    
    // 스테이지에서 값 받아오기 (추후에 값 수정)
    private int _stageCount => _stage.StageNumber; 
    private int _waveCount => _stage.WaveNumber; 
    private int _currentMonsterCount => _stage.monsterNumber;
    private int _maxMonsterCount => _stage.totalMonsterNumber;
    private int _getGoldCount => _monsterStat.PlusWaveGold;
    private int _stageRewardCount => _monsterStat.PlusStageGold;
    
    private void Start() => Result();
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
        gameObject.SetActive(false);
    }

    private void Lobby()
    {
        SceneManager.LoadScene(1);
    }

    private void Result()
    {
        _stageNumber.text = $"Stage # " + _stageCount.ToString();
        _waveNumber.text = $"Wave # " + _waveCount.ToString();
        _monsterCount.text = _currentMonsterCount.ToString() + " / " + _maxMonsterCount.ToString();
        _getGold.text = $"Gold : " + _getGoldCount.ToString();
        _stageReward.text = $"Reward : " + _stageRewardCount.ToString();
    }
}
