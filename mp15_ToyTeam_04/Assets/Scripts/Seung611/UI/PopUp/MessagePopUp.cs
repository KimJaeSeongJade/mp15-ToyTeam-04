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
    
    private KeyCode _messageKey = KeyCode.Space;
    private bool _messagePopup => Input.GetKeyDown(_messageKey);
    
    private string _notEnoughGold = "You don't have enough gold to buy the tower.";

    private void Update()
    {
        WarningMessage();
    }

    private void WarningMessage()
    {
        if (_messagePopup)
        {
            UIManager.Instance.PopUp.MessagePopUp.Message(_notEnoughGold);
        }
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
