using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessagePopUp : MonoBehaviour   
{
    private ObjectPool<Message> _messagePool;
    public Message _message;
    private void Start()
    {
        _messagePool = new ObjectPool<Message>(_message, 5, this.transform);
    }
    
    private string _notEnoughGold = "타워를 사기에 보유한 골드가 부족합니다.";
    private string _notSkillCoolTime = "스킬 쿨타임이 끝나지 않았습니다.";
    private string _notUpgradeTower = "타워가 이미 최대 레벨입니다.";
    

    private void Update()
    {
        WarningMessage();
    }

    private void WarningMessage()
    {
        /*if (GoldManager.Instance.UseGold(PlayerManager.Instance.GetInstallCost(PlayerManager.Instance.SelectedTowerType)))
        {
            UIManager.Instance.PopUp.MessagePopUp.Message(_notEnoughGold);
        }*/
    }

    public void Message(string text)
    {
        Message message = _messagePool.GetObject();
        if (message == null)
        {
            return;
        }
        message.SetData(text, _messagePool);
        _messagePool.ActivateObject(message);
    }
}
