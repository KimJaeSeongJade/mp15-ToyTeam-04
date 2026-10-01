using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Map : MonoBehaviour
{
    [Header("타워 타일")] public Tile[] TowerTile;
    [Header("몬스터 타일")] public Tile[] MonsterTile;

    [Header("몬스터 스폰 좌표"), SerializeField] private Transform _spawnPoint;
    public Transform SpawnPoint => _spawnPoint;
    [Header("몬스터 도착 좌표"), SerializeField] private Transform _arrivalPoint;
    public Transform ArrivalPoint => _arrivalPoint;

    /// <summary> 맵 타일 상태 초기화 </summary>
    public void ResetMap()
    {
        foreach(Tile tile in TowerTile)
        {
            tile.ResetTile();
        }
    }
}