using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITest : MonoBehaviour
{
    [SerializeField] private Image image;
    public Vector2 mp;
    
    void Start()
    {
        mp = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 localPosition = mp;
            mp = Input.mousePosition;
            Debug.Log(mp.ToString());
        }
    }
}
