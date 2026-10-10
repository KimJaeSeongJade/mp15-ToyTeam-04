using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Image _bg;
    [SerializeField] private Image _icon;
    [SerializeField] private Sprite[] _iconImg;
    [SerializeField] private TextMeshProUGUI _tipText;
    private string[] _tips;

    private void Start()
    {
        _tips = new string[5];
        _tips[0] = "애로우 타워는 기본 타워입니다.";
        _tips[1] = "파이어 타워는 강력한 타워입니다.";
        _tips[2] = "아이스 타워는 적을 느리게 만드는 타워입니다.";
        _tips[3] = "몬스터가 귀엽습니다!";
        _tips[4] = "타워 업그레이드는 3단계까지 있습니다.";
        _tipText.gameObject.SetActive(false);
        _icon.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void LoadingStart()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        _canvasGroup.alpha = 1f;
        _bg.gameObject.SetActive(true);

        float alpha = 0f;

        while(true)
        {
            alpha += Time.deltaTime;

            _bg.color = new Color(0, 0, 0, alpha);

            yield return null;

            if (alpha >= 1f)
                break;
        }

        _bg.color = new Color(0, 0, 0, 1f);

        int rand = Random.Range(0, _tips.Length);
        _tipText.text = _tips[rand];
        _tipText.gameObject.SetActive(true);

        int randImg = Random.Range(0, _iconImg.Length);
        _icon.sprite = _iconImg[randImg];
        _icon.gameObject.SetActive(true);

        UIManager.Instance.Window.LobbyWindowOpen();
        SceneManager.LoadScene(1);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FadeInAfterLoad());
    }

    private IEnumerator FadeInAfterLoad()
    {
        // 새 씬의 Start()가 실행될 때까지 기다립니다.
        yield return null;

        // 게임 씬에서는 풀 준비도 기다립니다.
        while (PoolManager.Instance != null &&
               !PoolManager.Instance.IsReady)
        {
            yield return null;
        }

        yield return StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float alpha = 1f;

        while (true)
        {
            alpha -= Time.deltaTime;

            _bg.color = new Color(0, 0, 0, alpha);
            _canvasGroup.alpha = alpha;

            yield return null;

            if (alpha <= 0f)
                break;
        }

        _bg.color = new Color(0, 0, 0, 0);
        _canvasGroup.alpha = 0;

        _bg.gameObject.SetActive(false);
        _tipText.gameObject.SetActive(false);
        _icon.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
