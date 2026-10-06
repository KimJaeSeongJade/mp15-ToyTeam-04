using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    // ���� ���� �� ���� ��� (Ÿ�� ���� ��ȭ���� �غ�����)
    private const int START_GOLD = 1500;  
    public static GoldManager Instance { get; private set; }

    private int _gold;

    public int Gold => _gold;       // ���� ���� ���(�о����)
    public event Action<int> OnGoldChanged;      // ��� ���� �� UI ���ſ�

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // ���� ��� �Ҹųֱ�
        _gold = START_GOLD;
    }

    // ��带 �����ÿ�
    // GoldManager.Instance.AddGold() �� �� �����ø� �˴ϴ�.
    public void AddGold(int amount)
    {
        _gold += amount;

        if (OnGoldChanged != null)
        {
            OnGoldChanged(_gold);
        }
        // �׽�Ʈ �α�
        Debug.Log($"��� {amount} �߰�  ���� {_gold}");
        UIManager.Instance.Window.LobbyWindow.HaveGlod(_gold);
        UIManager.Instance.Window.BattleWindow.HaveGlod(_gold);
    }

    // ��� ��� (��� ����Ҷ���)
    // GoldManager.Instance.UseGold() �� �� �����ø� �˴ϴ�.
    public bool UseGold(int amount)
    {
        if (_gold < amount)
        {
            // �׽�Ʈ �α�
            Debug.Log($" �ʿ��� ��� {amount}, ���� ��� {_gold}, ���� ���{amount - _gold} ");
            return false;
        }

        _gold -= amount;

        // ��� ���� �� UI ���źκ�
        if (OnGoldChanged != null)
        {
            OnGoldChanged(_gold);
        }
        // �׽�Ʈ �α�
        Debug.Log($"��徴�� {amount} ���� ���� ��� {_gold}");
        return true;
    }
}
