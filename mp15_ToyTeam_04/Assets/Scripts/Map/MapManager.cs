using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [Header("전체 맵"), SerializeField] private Map[] NormalMaps;
    [Header("보스 맵"), SerializeField] private Map BossMap;
    [HideInInspector] public Map _curMap;

    /// <summary> 현재 맵 번호 </summary>
    private int _curMapNumber;

    private void Start()
    {
        // 시작 시 맵 정보X
        _curMapNumber = -1;
        _curMap = null;
    }

    /// <summary> 배틀 맵 보여주기 </summary>
    public void ShowBattleMap()
    {
        // 처음 시작 시 맵이 없기 때문에 Null 조건부 연산자로 체크
        // 맵 초기화
        _curMap?.ResetMap();

        // 맵은 랜덤으로 하기 때문에 랜덤 숫자
        int rand = Random.Range(0, NormalMaps.Length);

        // 같은 값이면 비활성 안하기 위해 나가기
        if (rand == _curMapNumber) return;

        // 초기 맵이 아니라면
        if (_curMapNumber != -1)
        {
            // 전 맵 비활성화
            NormalMaps[_curMapNumber].gameObject.SetActive(false);
        }

        // 현재 맵 번호 갱신
        _curMapNumber = rand;
        // 맵 활성화
        NormalMaps[_curMapNumber].gameObject.SetActive(true);
        // 현재 맵 정보
        _curMap = NormalMaps[_curMapNumber];
    }
}