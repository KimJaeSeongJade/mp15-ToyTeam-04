using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private bool _destroyOnLoad;
    
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
            }
            return _instance;
        }
    }

    /// <summary> 싱글톤 설정 함수. Awake에서 호출. </summary>
    protected void SetSingleton()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this as T;
            DestroyOnLoad();
        }
    }

    private void DestroyOnLoad()
    {
        if (_destroyOnLoad) return;
        DontDestroyOnLoad(gameObject);
    }
}
