using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] _bgmClips;
    [SerializeField] private AudioClip[] _sfxClips;
    
    [Header("Audio Sources")]
    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioSource _sfxSource;

    private Dictionary<EBgm, AudioClip> _bgmDict;
    private Dictionary<ESfx, AudioClip> _sfxDict;

    private void Awake()
    {
        SetSingleton();
        Init();
    }

    private void Init()
    {
        // BGM 초기화
        _bgmDict = new Dictionary<EBgm, AudioClip>();
        for (int i = 0; i < _bgmClips.Length; i++)
        {
            _bgmDict[(EBgm)i] = _bgmClips[i];
        }
        
        // SFX 초기화
        _sfxDict = new Dictionary<ESfx, AudioClip>();
        for (int i = 0; i < _sfxClips.Length; i++)
        {
            _sfxDict[(ESfx)i] = _sfxClips[i];
        }
    }

    public void PlayBgm(EBgm ebgmType)
    {
        if (_bgmDict.TryGetValue(ebgmType, out var clip))
        {
            _bgmSource.clip = clip;
            _bgmSource.loop = true;
            _bgmSource.Play();
        }
        else
        {
            Debug.Log("NOT FOUND BGM");
        }
    }

    public void PlaySfx(ESfx esfxType)
    {
        if (_sfxDict.TryGetValue(esfxType, out var clip))
        {
            _sfxSource.clip = clip;
            //_sfxSource.loop = true;
            _sfxSource.Play();
            //_sfxSource.PlayOneShot(clip); // 여러번 호출해도 독립적으로 재생됨
        }
        else
        {
            Debug.Log("NOT FOUND SFX");
        }
    }
}

// Sound 목록 Enum
public enum EBgm
{
    TITLE,
    LOBBY,
    BATTLE,
    CLEAR,
    FALE
}

public enum ESfx
{
    BUTTON_CLICK,
    CAMERA_MOVE,
    ARROW_ATK,
    FIRE_ATK,
    ICE_ATK,
    GET_GOLD,
    USE_GOLD,
    MONSTER_HIT,
    MONSTER_DIE,
    MONSTER_POTAL_END,
    MONSTER_POTAL_START,
    MOVE,
    SKILL_ALL_ATK,
    SKILL_TIME_FREZZ,
    TOWER_INSTALL,
    TOWER_REMOVE,
    TOWER_UPGRADE
}