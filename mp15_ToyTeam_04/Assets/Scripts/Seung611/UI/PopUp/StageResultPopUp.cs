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
    private int _stageCount = 0; 
    private int _waveCount = 0; 
    private int _currentMonsterCount = 0;
    private int _maxMonsterCount = 0;
    private int _getGoldCount = 0;
    private int _stageRewardCount = 0;
    
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
        // SceneManager.LoadScene(); // 내 다음씬 불러오기 (랜덤으로)
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

    private void Esc()
    {
        gameObject.SetActive(false);
    }
}
