using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Debug = UnityEngine.Debug;
using Image = UnityEngine.UI.Image;

public class TowerSpecPopUp : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _towerName;
    [SerializeField] private TextMeshProUGUI _towerDescription;
    [SerializeField] private List<TextMeshProUGUI> _towerSpec;
    [SerializeField] private List<Button> _towerButton;
    [SerializeField] private Image[] _towerImage;
    [SerializeField] private Button _escButton;
    
    private TowerState[] _towerStates;
    private TowerState _tower;
    private int _index;

    [SerializeField] private Sprite[] _arrowTowerSprites;
    [SerializeField] private Sprite[] _fireTowerSprites;
    [SerializeField] private Sprite[] _iceTowerSprites;
    
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
    
    public void SetData(TowerState[] stat, int index)
    {
        _towerStates = stat;
        _index = index;
        TowerSelectButton(_towerStates[index]);
        SpriteChange(index);
    }

    private void Tower0Select()
    {
        _towerStates[0] = PlayerManager.Instance.GetTowerState(ETowerType.ArrowTower);
        TowerSelectButton(_towerStates[0]);
        SpriteChange(0);
        UIManager.Instance.PopUp.TowerSpecPopUpOpen();
    }
    
    private void Tower1Select()
    {
        _towerStates[1] = PlayerManager.Instance.GetTowerState(ETowerType.FireTower);
        TowerSelectButton(_towerStates[1]);
        SpriteChange(1);
        UIManager.Instance.PopUp.TowerSpecPopUpOpen();
    }
    
    private void Tower2Select()
    {
        _towerStates[2] = PlayerManager.Instance.GetTowerState(ETowerType.IceTower);
        TowerSelectButton(_towerStates[2]);
        SpriteChange(2);
        UIManager.Instance.PopUp.TowerSpecPopUpOpen();
    }

    private void TowerSelectButton(TowerState state)
    {
        _towerName.text = state.Name;
        _towerDescription.text = state.Explanation;
    }

    private void SpriteChange(int index)
    {
        switch (index)
        {
            case 0:
                for (int i = 0; i < _towerImage.Length; i++)
                {
                    _towerImage[i].sprite = _arrowTowerSprites[i];
                }
                _towerSpec[0].text = $"공격력 : {_towerStates[0].Atk} \n " +
                                     $"설치 비용 : {_towerStates[0].InstallCost}";
                _towerSpec[1].text = $"공격력 : {_towerStates[0].Atk} \n " +
                                     $"업그레이드 비용 : {_towerStates[0].FirstUpgradeCost}";
                _towerSpec[2].text = $"공격력 : {_towerStates[0].Atk} \n " +
                                     $"업그레이드 비용 : {_towerStates[0].SecondUpgradeCost}";
                
                break;
            case 1:
                for (int i = 0; i < _towerImage.Length; i++)
                {
                    _towerImage[i].sprite = _fireTowerSprites[i];
                }
                _towerSpec[0].text = $"공격력 : {_towerStates[1].Atk} \n " +
                                     $"설치 비용 : {_towerStates[1].InstallCost}";
                _towerSpec[1].text = $"공격력 : {_towerStates[1].Atk} \n " +
                                     $"업그레이드 비용 : {_towerStates[1].FirstUpgradeCost}";
                _towerSpec[2].text = $"공격력 : {_towerStates[1].Atk} \n " +
                                     $"업그레이드 비용 : {_towerStates[1].SecondUpgradeCost}";
                break;
            case 2:
                for (int i = 0; i < _towerImage.Length; i++)
                {
                    _towerImage[i].sprite = _iceTowerSprites[i];
                }
                _towerSpec[0].text = $"공격력 : {_towerStates[2].Atk} \n " +
                                     $"설치 비용 : {_towerStates[2].InstallCost}";
                _towerSpec[1].text = $"공격력 : {_towerStates[2].Atk} \n " +
                                     $"업그레이드 비용 : {_towerStates[2].FirstUpgradeCost}";
                _towerSpec[2].text = $"공격력 : {_towerStates[2].Atk} \n " +
                                     $"업그레이드 비용 : {_towerStates[2].SecondUpgradeCost}";
                break;
            default:
                break;
        }
    }

    private void Esc()
    {
        gameObject.SetActive(false);
    }
}
