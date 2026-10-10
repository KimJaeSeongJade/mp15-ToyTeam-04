using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerSelectTilePopUp : MonoBehaviour
{
    [SerializeField] private Button _installButton;
    [SerializeField] private Button _uninstallButton;
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private Button _cancelButton;
    
    private void OnEnable() => BindEventButtons();
    private void OnDisable() => UnbindEventButtons();

    private void BindEventButtons()
    {
        _installButton.onClick.AddListener(InstallButton);
        _uninstallButton.onClick.AddListener(UninstallButton);
        _upgradeButton.onClick.AddListener(UpgradeButton);
        _cancelButton.onClick.AddListener(CancelButton);
    }

    private void UnbindEventButtons()
    {
        _installButton.onClick.RemoveListener(InstallButton);
        _uninstallButton.onClick.RemoveListener(UninstallButton);
        _upgradeButton.onClick.RemoveListener(UpgradeButton);
        _cancelButton.onClick.RemoveListener(CancelButton);
    }

    public void NoTower()
    {
        _uninstallButton.gameObject.SetActive(false);
        _upgradeButton.gameObject.SetActive(false);
    }

    public void YesTower()
    {
        _installButton.gameObject.SetActive(false);
    }

    private void InstallButton()
    {
        // 설치 버튼을 눌렀을 때 오른쪽 타워 버튼을 누르면 버튼에 맞는 타워가 설치 됨
        SoundManager.Instance.PlaySfx(ESfx.BUTTON_CLICK);
        UIManager.Instance.Window.BattleWindow.PushInstallButton();
        PlayerManager.Instance.TopViewSelect.SetActive(false);
    }

    private void UninstallButton()
    {
        // 타워 파괴 되면서 돈이 들어옴
        PlayerManager.Instance.DemolishTower();
        PlayerManager.Instance.TopViewSelect.SetActive(false);
        CancelButton();
    }

    private void UpgradeButton()
    {
        // 타워 이미지 바뀌고 능력도 바뀜
        SoundManager.Instance.PlaySfx(ESfx.BUTTON_CLICK);
        PlayerManager.Instance.UpgradeTower();
        PlayerManager.Instance.TopViewSelect.SetActive(false);
    }

    public void CancelButton()
    {
        // 타워 설치 취소 인게임 화면으로 다시 돌아감
        SoundManager.Instance.PlaySfx(ESfx.BUTTON_CLICK);
        gameObject.SetActive(false);
        UIManager.Instance.Window.BattleWindow.HideInstallButton();
        PlayerManager.Instance.SetUIMode(false);
        Init();
        PlayerManager.Instance.TopViewSelect.SetActive(false);
    }

    public void TopView()
    {
        Tile tile = PlayerManager.Instance.SelectedTile;

        Vector3 selectTile = new Vector3(
            tile.gameObject.transform.position.x,
            tile.gameObject.transform.position.y + 1.5f,
            tile.gameObject.transform.position.z
            );

        PlayerManager.Instance.TopViewSelect.SetActive(true);
        PlayerManager.Instance.TopViewSelect.transform.position = selectTile;

        if(tile.IsTower)
        {
            YesTower();
        }
        else
        {
            NoTower();
        }

        //UIManager.Instance.PopUp.TopViewTowerSelectTilePopUp.transform.position = selectTile;
        //Debug.Log(UIManager.Instance.PopUp.TopViewTowerSelectTilePopUp.transform.position);
    }

    private void Init()
    {
        _uninstallButton.gameObject.SetActive(true);
        _upgradeButton.gameObject.SetActive(true);
        _installButton.gameObject.SetActive(true);
    }
}
