using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Message : MonoBehaviour, IPoolable
{
    [SerializeField] private TextMeshProUGUI _messageText;
    
    private ObjectPool<Message> _pool;

    public void SetData(string message, ObjectPool<Message> pool)
    {
        _messageText.text = message;
        _pool = pool;
    }
    
    public void OnSpawn()
    {
        StartCoroutine(MessagePopUpRoutine());
    }

    public void OnDespawn()
    {
        StopCoroutine(MessagePopUpRoutine());
    }

    private IEnumerator MessagePopUpRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("0.5초 기다려");
        _pool.ReturnObject(this);
    }
}
