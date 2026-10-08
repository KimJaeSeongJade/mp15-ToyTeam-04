using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    // 게임 시작 시 지급 골드 (타워 짓고 강화까지 해볼려구)
    private const int START_GOLD = 1500;  
    public static GoldManager Instance { get; private set; }

    private int _gold;

    public int Gold => _gold;       // 현재 소지 골드(읽어오기)
    public event Action<int> OnGoldChanged;      // 골드 변경 시 UI 갱신용

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

    }

    private void Start()
    {
        // 시작 골드 소매넣기
        _gold = START_GOLD;
        GoldView();
    }

    private void GoldView()
    {
        UIManager.Instance.Window.LobbyWindow.HaveGold(_gold);
        UIManager.Instance.Window.BattleWindow.HaveGold(_gold);
    }

    // 골드를 얻을시에
    // GoldManager.Instance.AddGold() 로 값 넣으시면 됩니다.
    public void AddGold(int amount)
    {
        _gold += amount;

        if (OnGoldChanged != null)
        {
            OnGoldChanged(_gold);
        }
        // 테스트 로그
        Debug.Log($"골드 {amount} 추가  현재 {_gold}");
        GoldView();
    }

    // 골드 사용 (골드 충분할때만)
    // GoldManager.Instance.UseGold() 로 값 넣으시면 됩니다.
    public bool UseGold(int amount)
    {
        if (_gold < amount)
        {
            // 테스트 로그
            Debug.Log($" 필요한 골드 {amount}, 보유 골드{_gold}, 부족 골드{amount - _gold} 입니다 ");
            return false;
        }

        _gold -= amount;

        // 골드 변경 시 UI 갱신부분
        if (OnGoldChanged != null)
        {
            OnGoldChanged(_gold);
        }
        // 테스트 로그
        Debug.Log($"{amount}골드 사용  남은 골드 {_gold} 입니다");
        GoldView();
        return true;
    }
}
