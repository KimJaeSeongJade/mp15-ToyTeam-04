using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("타일 번호"), SerializeField] private int _tileNumber;
    public int TileNumber => _tileNumber;

    [Header("타일 속성"), SerializeField] private ETileType _eTileType;
    public ETileType ETileType => _eTileType;

    // [HideInInspector] 
    public Tower _tower;

    // 타일에 타워 존재 여부
    private bool _isTower;
    public bool IsTower => _isTower;

    private void Start()
    {
        _isTower = false;
    }

    /// <summary> 타일 타워 건설 </summary>
    /// <param name="tower"></param>
    public void TileInstallTower(TowerState state, TowerAbility towerAbility)
    {
        _isTower = true;
        // 타워 생성
        // 타워 위치
        // 타워 초기값 주기
        _tower.gameObject.transform.position = transform.position;
        _tower.TowerBasicInstall(state, towerAbility);
    }

    /// <summary> 타일 타워 제거 </summary>
    public void TileRemovalTower()
    {
        _tower.TowerRemoval();
        ResetTile();
    }

    /// <summary> 타일 상태 초기화 </summary>
    public void ResetTile()
    {
        _isTower = false;
        _tower = null;
    }
}