using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("타일 번호"), SerializeField] private int _tileNumber;
    public int TileNumber => _tileNumber;

    [Header("타일 속성"), SerializeField] private ETileType _eTileType;
    public ETileType ETileType => _eTileType;

    //[HideInInspector] public Tower _tower;

    // 타일에 타워 존재 여부
    private bool _isTower;
    public bool IsTower => _isTower;

    private void Start()
    {
        _isTower = false;
    }

    /*
    // 타일에 타워 설치
    public void TileInstallTower(Tower tower)
    {
        _isTower = true;
        _tower = tower;
    }

    // 타일에 타워 철거
    public void TileRemovalTower()
    {
        _isTower = false;
        _tower = null;
    }
    */
}