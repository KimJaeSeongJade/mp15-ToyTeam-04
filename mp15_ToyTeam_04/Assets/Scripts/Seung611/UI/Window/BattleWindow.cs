using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleWindow : MonoBehaviour
{
    [SerializeField] private Image _playerSkill;
    [SerializeField] private Image _playerSkillCoolDown;
    [SerializeField] private TextMeshProUGUI _playerSkillCoolDownCount;
    [SerializeField] private Button _settingButton;
    [SerializeField] private List<Button> _towerInventory;
    [SerializeField] private TextMeshProUGUI _haveGlod;
    [SerializeField] private TextMeshProUGUI _monsterCount;
    
    private Image _skillButtonImage => UIManager.Instance.Window.LobbyWindow._playerSkillButton.image;
    private void Awake() => Init();
    private void OnEnable() => BindButtonEvents();
    private void OnDisable() => UnbindButtonEvents();

    private void Update()
    {
        CurrnetMonster();
    }

    // Start 버튼 눌렀을 때 Battle 화면으로
    private void BindButtonEvents()
    {
        _settingButton.onClick.AddListener(SettingPopUp);
        _towerInventory[0].onClick.AddListener(Tower0Select);
        _towerInventory[1].onClick.AddListener(Tower1Select);
        _towerInventory[2].onClick.AddListener(Tower2Select);
    }
    
    private void UnbindButtonEvents()
    {
        _settingButton.onClick.RemoveListener(SettingPopUp);
        _towerInventory[0].onClick.RemoveListener(Tower0Select);
        _towerInventory[1].onClick.RemoveListener(Tower1Select);
        _towerInventory[2].onClick.RemoveListener(Tower2Select);
    }
 
    public void SkillPopUp()
    {
        _playerSkillCoolDown.gameObject.SetActive(true);
        _playerSkillCoolDownCount.gameObject.SetActive(true);
        StartCoroutine(SkillUsingRoutin());
    }

    private IEnumerator SkillUsingRoutin()
    {
        while (PlayerManager.Instance.SkillCoolTimer > 0)
        {
            _playerSkillCoolDown.fillAmount = PlayerManager.Instance.SkillCoolTimer / 60f;
            _playerSkillCoolDownCount.text = Mathf.Round(PlayerManager.Instance.SkillCoolTimer).ToString();
            yield return null;
        }
        _playerSkillCoolDown.gameObject.SetActive(false);
        _playerSkillCoolDownCount.gameObject.SetActive(false);
    }

    private void SettingPopUp()
    {
        UIManager.Instance.PopUp.SettingPopUpOpen();
    }

    public void PushInstallButton()
    {
        _towerInventory[0].gameObject.SetActive(true);
        _towerInventory[1].gameObject.SetActive(true);
        _towerInventory[2].gameObject.SetActive(true);
    }

    private void Tower0Select()
    {
        // ArrowTower 설치
        PlayerManager.Instance.InstallTower(ETowerType.ArrowTower);
        InstallComplete();
    }


    private void Tower1Select()
    {
        // FireTower 설치
        PlayerManager.Instance.InstallTower(ETowerType.FireTower);
        InstallComplete();
    }

    private void Tower2Select()
    {
        // IceTower 설치
        PlayerManager.Instance.InstallTower(ETowerType.IceTower);
        InstallComplete();
    }
    private void InstallComplete()
    {
        UIManager.Instance.PopUp.TowerSelectTilePopUp.CancelButton();
        UIManager.Instance.Window.BattleWindow.HideInstallButton();
    }

    private void Init()
    {
        _playerSkill.sprite = _skillButtonImage.sprite;
        _playerSkillCoolDown.gameObject.SetActive(false);
        _playerSkillCoolDownCount.gameObject.SetActive(false);
        HideInstallButton();
    }

    public void HideInstallButton()
    {
        _towerInventory[0].gameObject.SetActive(false);
        _towerInventory[1].gameObject.SetActive(false);
        _towerInventory[2].gameObject.SetActive(false);
    }

    public void CurrnetMonster()
    {
        int _currentMonster = StageManager.Instance.monsterNumber;
        int _maxMonster = StageManager.Instance.totalMonsterNumber;
        _monsterCount.text = _currentMonster.ToString() + " / " + _maxMonster.ToString();
    }

    public void HaveGold(int gold)
    {
        _haveGlod.text = "Gold : " + gold.ToString();
    }
}