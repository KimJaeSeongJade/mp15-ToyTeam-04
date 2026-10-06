using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class TowerSpecPopUp : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _towerName;
    [SerializeField] private TextMeshProUGUI _towerDescription;
    [SerializeField] private List<Button> _towerButton;
    [SerializeField] private List<Image> _towerImage;
    [SerializeField] private Button _escButton;
    
    private TowerState[] _towerStates;
    private TowerState _tower;
    private int _index;
    
    private void OnEnable() => BindEventButtons();
    private void OnDisable() => UnbindEventButtons();
    
    private void BindEventButtons()
    {
        _towerButton[0].onClick.AddListener(Tower0Select);
        _towerButton[1].onClick.AddListener(Tower1Select);
        _towerButton[2].onClick.AddListener(Tower2Select);
        _escButton.onClick.AddListener(Esc);
    }

    private void UnbindEventButtons()
    {
        _towerButton[0].onClick.RemoveListener(Tower0Select);
        _towerButton[1].onClick.RemoveListener(Tower1Select);
        _towerButton[2].onClick.RemoveListener(Tower2Select);
        _escButton.onClick.RemoveListener(Esc);
    }

    private void SelectTower()
    {
        gameObject.SetActive(false);
        // 선택된 타워 설지하는 것과 연동
    }
    
    public void SetData(TowerState[] stat, int index)
    {
        _towerStates = stat;
        _index = index;
        TowerSelectButton(_towerStates[index]);
    }

    private void Tower0Select()
    {
        _towerStates[0] = new ArrowTower();
        TowerSelectButton(_towerStates[0]);
        UIManager.Instance.PopUp.TowerSpecPopUpOpen(0);
    }
    
    private void Tower1Select()
    {
        _towerStates[1] = new FireTower();
        TowerSelectButton(_towerStates[1]);
        UIManager.Instance.PopUp.TowerSpecPopUpOpen(1);
    }
    
    private void Tower2Select()
    {
        _towerStates[2] = new IceTower();
        TowerSelectButton(_towerStates[2]);
        UIManager.Instance.PopUp.TowerSpecPopUpOpen(2);
    }

    private void TowerSelectButton(TowerState _state)
    {
        _towerName.text = _state.Name;
        _towerDescription.text = _state.Explanation;
    }

    private void Esc()
    {
        gameObject.SetActive(false);
    } 
}
