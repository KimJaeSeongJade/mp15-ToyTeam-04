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

    private void InstallButton()
    {
        Debug.Log("타워 설치");
    }

    private void UninstallButton()
    {
        Debug.Log("타워 파괴");
    }

    private void UpgradeButton()
    {
        Debug.Log("타워 강화");
    }

    private void CancelButton()
    {
        Debug.Log("취소");
    }
}
