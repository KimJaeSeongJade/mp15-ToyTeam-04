using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TowerSpecPopUp : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _towerName;
    [SerializeField] private TextMeshProUGUI _towerDescription;
    [SerializeField] private Button _tower1Button;
    [SerializeField] private Button _tower2Button;
    [SerializeField] private Button _tower3Button;
    [SerializeField] private Button _selectButton;
    [SerializeField] private Button _escButton;
    
    private bool _tower1Selected = false;
    private bool _tower2Selected = false;
    private bool _tower3Selected = false;
    
    private void OnEnable() => BindEventButtons();
    private void OnDisable() => UnbindEventButtons();

    private void BindEventButtons()
    {
        _tower1Button.onClick.AddListener(Tower1Selected);
        _tower2Button.onClick.AddListener(Tower2Selected);
        _tower3Button.onClick.AddListener(Tower3Selected);
        _selectButton.onClick.AddListener(SelectTower);
        _escButton.onClick.AddListener(Esc);
    }

    private void UnbindEventButtons()
    {
        _tower1Button.onClick.RemoveListener(Tower1Selected);
        _tower2Button.onClick.RemoveListener(Tower2Selected);
        _tower3Button.onClick.RemoveListener(Tower3Selected);
        _selectButton.onClick.RemoveListener(SelectTower);
        _escButton.onClick.RemoveListener(Esc);
    }

    private void SelectTower()
    {
        gameObject.SetActive(false);
        // 선택된 타워 설지하는 것과 연동
    }
    
    private void Tower1Selected()
    {
        _tower1Selected = true;
        TowerSelectButton();
        _tower1Selected = false;
    }

    private void Tower2Selected()
    {
        _tower2Selected = true;
        TowerSelectButton();
        _tower2Selected = false;
    }

    private void Tower3Selected()
    {
        _tower3Selected = true;
        TowerSelectButton();
        _tower3Selected = false;
    }

    private void TowerSelectButton()
    {
        if (_tower1Selected)
        {
            _towerName.text = $"ArrowTower";
            _towerDescription.text = $"ArrowTower Description";
        }
        else if (_tower2Selected)
        {
            _towerName.text = $"FireTower";
            _towerDescription.text = $"FireTower Description";
        }
        else if (_tower3Selected)
        {
            _towerName.text = $"IceTower";
            _towerDescription.text = $"IceTower Description";
        }
    }

    private void Esc()
    {
        gameObject.SetActive(false);
    } 
}
