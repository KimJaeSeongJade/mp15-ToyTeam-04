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

    // 타워 스택창
    [SerializeField] private GameObject _towerSpecObj;
    [SerializeField] private List<TextMeshProUGUI> _towerSpec;
    [SerializeField] private List<Image> _towerButtonImg;
    [SerializeField] private List<Image> _towerLevelButtonImg;
    [SerializeField] private Sprite _deselectButton;
    [SerializeField] private Sprite _selectButton;

    // 타워 특성창
    [SerializeField] private GameObject _towerAbilityObj;
    private TowerAbility _towerAbility;
    [SerializeField] private Image[] _abilityImg;
    [SerializeField] private Sprite[] _arrowAbilitySprite;
    [SerializeField] private Sprite[] _fireAbilitySprite;
    [SerializeField] private Sprite[] _iceAbilitySprite;
    [SerializeField] private TextMeshProUGUI _abilityName;
    [SerializeField] private TextMeshProUGUI _learnNeedGold;
    [SerializeField] private TextMeshProUGUI _learn;
    [SerializeField] private Image _learnButtonImg;
    [SerializeField] private Sprite _noLearnSprite;
    [SerializeField] private Sprite _learnSprite;
    [SerializeField] private TextMeshProUGUI _abilityExplan;



    [SerializeField] private List<Button> _towerButton;
    [SerializeField] private Image _towerImage;
    [SerializeField] private Button _escButton;

    private TowerState[] _towerStates;
    private TowerState _tower;
    private int _index;
    private int _levelIndex;
    private int _abilityIndex;

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

    public void SetData(TowerState[] stat, TowerAbility ability, int index)
    {
        _towerSpecObj.SetActive(true);
        _towerAbilityObj.SetActive(false);
        _towerStates = stat;
        _index = index;
        SelectButtonImg(_index);
        SelectButtonTowerLevelImg(0);
        _abilityIndex = -1;
        _towerAbility = ability;
        TowerSelectButton(_towerStates[index]);
        SpriteChange(index, 0);
    }

    private void Tower0Select()
    {
        SoundManager.Instance.PlaySfx(ESfx.BUTTON_CLICK);
        _index = 0;
        _towerStates[0] = PlayerManager.Instance.GetTowerState(ETowerType.ArrowTower);
        _towerAbility = PlayerManager.Instance.GetTowerAbility(ETowerType.ArrowTower);
        TowerSelectButton(_towerStates[0]);
        SpriteChange(0, 0);
        SelectButtonImg(0);
        SelectButtonTowerLevelImg(0);
        UIManager.Instance.PopUp.TowerSpecPopUpOpen();
    }

    private void Tower1Select()
    {
        SoundManager.Instance.PlaySfx(ESfx.BUTTON_CLICK);
        _index = 1;
        _towerStates[1] = PlayerManager.Instance.GetTowerState(ETowerType.FireTower);
        _towerAbility = PlayerManager.Instance.GetTowerAbility(ETowerType.FireTower);
        TowerSelectButton(_towerStates[1]);
        SpriteChange(1, 0);
        SelectButtonImg(1);
        SelectButtonTowerLevelImg(0);
        UIManager.Instance.PopUp.TowerSpecPopUpOpen();
    }

    private void Tower2Select()
    {
        SoundManager.Instance.PlaySfx(ESfx.BUTTON_CLICK);
        _index = 2;
        _towerStates[2] = PlayerManager.Instance.GetTowerState(ETowerType.IceTower);
        _towerAbility = PlayerManager.Instance.GetTowerAbility(ETowerType.IceTower);
        TowerSelectButton(_towerStates[2]);
        SpriteChange(2, 0);
        SelectButtonImg(2);
        SelectButtonTowerLevelImg(0);
        UIManager.Instance.PopUp.TowerSpecPopUpOpen();
    }

    private void SelectButtonImg(int index)
    {
        for (int i = 0; i < _towerButtonImg.Count; i++)
        {
            if (i == index)
            {
                _towerButtonImg[i].sprite = _selectButton;
            }
            else
            {
                _towerButtonImg[i].sprite = _deselectButton;
            }
        }
    }

    public void SelectButtonTowerLevelImg(int index)
    {
        _levelIndex = index;

        for (int i = 0; i < _towerLevelButtonImg.Count; i++)
        {
            if (i == _levelIndex)
            {
                _towerLevelButtonImg[i].sprite = _selectButton;
            }
            else
            {
                _towerLevelButtonImg[i].sprite = _deselectButton;
            }
        }

        SpriteChange(_index, _levelIndex);
    }

    public void TowerExAbility(int index)
    {
        _towerSpecObj.SetActive(index == 0);
        _towerAbilityObj.SetActive(index == 1);

        if(_abilityIndex == -1)
        {
            AbilitySelectButton(0);
        }
    }

    private void TowerSelectButton(TowerState state)
    {
        _towerName.text = state.Name;
        _towerDescription.text = state.Explanation;

        switch (state.ETowerType)
        {
            case ETowerType.ArrowTower:
                for(int i = 0; i < _abilityImg.Length; i++)
                {
                    _abilityImg[i].sprite = _arrowAbilitySprite[i];
                }
                break;
            case ETowerType.FireTower:
                for (int i = 0; i < _abilityImg.Length; i++)
                {
                    _abilityImg[i].sprite = _fireAbilitySprite[i];
                }
                break;
            case ETowerType.IceTower:
                for (int i = 0; i < _abilityImg.Length; i++)
                {
                    _abilityImg[i].sprite = _iceAbilitySprite[i];
                }
                break;
        }

        AbilitySelectButton(_towerAbilityObj.activeSelf ? _abilityIndex : 0);
    }

    public void AbilitySelectButton(int index)
    {
        SoundManager.Instance.PlaySfx(ESfx.BUTTON_CLICK);
        _abilityIndex = index;
        _abilityName.text = _towerAbility.DicAbility[(EAbilityType)_abilityIndex].Name;
        _learnNeedGold.text = _towerAbility.DicAbility[(EAbilityType)_abilityIndex].Cost.ToString();
        _learn.text = _towerAbility.DicAbility[(EAbilityType)_abilityIndex].IsLearn ? "습득 완료" : "습득 하기";
        _learnButtonImg.sprite = _towerAbility.DicAbility[(EAbilityType)_abilityIndex].IsLearn ? _learnSprite : _noLearnSprite;

        _abilityExplan.text = _towerAbility.DicAbility[(EAbilityType)_abilityIndex].Description;
    }

    public void AbilityLearn()
    {
        SoundManager.Instance.PlaySfx(ESfx.BUTTON_CLICK);
        if (!PlayerManager.Instance.GetTowerAbility((ETowerType)_index).DicAbility[(EAbilityType)_abilityIndex].IsLearn)
        {
            if (GoldManager.Instance.UseGold(PlayerManager.Instance.GetTowerAbility((ETowerType)_index).DicAbility[(EAbilityType)_abilityIndex].Cost))
            {
                PlayerManager.Instance.SetTowerAbility((ETowerType)_index, (EAbilityType)_abilityIndex);
                AbilitySelectButton(_abilityIndex);
                UIManager.Instance.PopUp.MessagePopUp.Message($"{_towerAbility.DicAbility[(EAbilityType)_abilityIndex].Name} 특성 습득!");
            }
            else
            {
                UIManager.Instance.PopUp.MessagePopUp.Message("골드가 부족합니다.");
            }
        }
        else
        {
            UIManager.Instance.PopUp.MessagePopUp.Message("이미 습득한 특성입니다.");
        }
    }

    private void SpriteChange(int index, int level)
    {
        if (index == 0)
            _towerImage.sprite = _arrowTowerSprites[level];
        else if (index == 1)
            _towerImage.sprite = _fireTowerSprites[level];
        else if (index == 2)
            _towerImage.sprite = _iceTowerSprites[level];

        _towerSpec[0].text = $"공 격 력  : {_towerStates[index].Atk}" + (level == 0 ? "" : $" (<color=#00FF00>+ {_towerStates[index].UpgradeData.AddAtk * level}</color>)");
        _towerSpec[1].text = $"공격 속도 : {_towerStates[index].AtkSpeed}" + (level == 0 ? "" : $" (<color=#00FF00>+ {_towerStates[index].UpgradeData.AddAtkSpeed * level}</color>)");
        _towerSpec[2].text = $"공격 범위 : {_towerStates[index].DetectionRange}M" + (level == 0 ? "" : $" (<color=#00FF00>+ {_towerStates[index].UpgradeData.AddRange * level}</color>M)");
        _towerSpec[3].text = $"공격 효과 : {_towerStates[index].Affect.AffectName(_towerStates[index].Affect.EBulletAffectType)}";

        if (level == 0)
        {
            _towerSpec[4].text = $"설치 비용 : {_towerStates[index].InstallCost} 골드";
        }
        else if (level == 1)
        {
            _towerSpec[4].text = $"강화 비용 : {_towerStates[index].FirstUpgradeCost} 골드";
        }
        else if (level == 2)
        {
            _towerSpec[4].text = $"강화 비용 : {_towerStates[index].SecondUpgradeCost} 골드";
        }
    }

    private void Esc()
    {
        gameObject.SetActive(false);
    }
}
