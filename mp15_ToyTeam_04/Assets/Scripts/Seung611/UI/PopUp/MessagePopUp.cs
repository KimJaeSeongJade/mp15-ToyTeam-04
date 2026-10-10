using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessagePopUp : MonoBehaviour   
{
    private ObjectPool<Message> _messagePool;
    [SerializeField] private GridLayoutGroup _gridLayout;
    public Message _message;
    private void Start()
    {
        _messagePool = new ObjectPool<Message>(_message, 5, this.transform);
    }
    
    // private string _notEnoughGold = "타워를 사기에 보유한 골드가 부족합니다.";
    // private string _notSkillCoolTime = "스킬 쿨타임이 끝나지 않았습니다.";
    // private string _notUpgradeTower = "타워가 이미 최대 레벨입니다.";
    
    // 각자 메시지 띄워야 하는 곳 찾아서 적으시오...
    // 위에 문자는 예시

    public void Message(string text)
    {
        _gridLayout.enabled = true;
        Message message = _messagePool.GetObject();
        if (message == null)
        {
            return;
        }
        message.SetData(text, _messagePool);
        _messagePool.ActivateObject(message);
        _gridLayout.enabled = false;
    }
}
