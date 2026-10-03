using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessagePopUp : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _messageText;
    
    private void Update() => Message();

    // private bool _notPayTower; 구매하고 싶은 타워가 가지고 있는 골드보다 더 클 때
    // private bool // 
    private string _notEnoughGold = "You don't have enough gold to buy the tower.";

    private void Message()
    {
        _messageText.text = _notEnoughGold;
        /*if () bool 값 넣기
        {
            _messageText.text = _notEnoughGold;
        }
        else if ()
        {
            
        }*/
    }
}
