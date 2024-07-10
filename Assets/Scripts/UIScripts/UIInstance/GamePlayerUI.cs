using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayerUI : UIBase
{
    private bool playing = false;
    private SettingUI _settingButton;
    private SettingUI _storeButton;
    private Image _blackImage;

    private ScoreBarUI _scoreBarUI;
    private ScoreTextUI _scoreTextUI;

    private CanvasGroup _blackTransitionsCanvesGroup;
    private Image _whiteTransitionsImage;

    private CanvasGroup _guideImagesCanvesGroup;

    public ScoreBarUI ScoreBarUIView { get => _scoreBarUI; }
    public ScoreTextUI ScoreTextUIView { get => _scoreTextUI; }

    Image _maskImage;
    //Image _maskImage2;

    private PlayerManager _playerManager;

    private TMP_Text _respawnText;
    private Button _respawnButton;
    private CanvasGroup _respawnCanvesGroup;
    public override void Init()
    {

        var rectTransform = _transform.GetComponent<RectTransform>();
        var temp = rectTransform.anchoredPosition;
        temp.y -= Screen.height - Screen.safeArea.height;
        rectTransform.anchoredPosition = temp;

        _transform.GetComponent<RectTransform>().anchoredPosition = temp;
        _settingButton = _transform.Find("SettingViewport").GetComponent<SettingUI>();
        _storeButton = _transform.Find("QuitViewport").GetComponent<SettingUI>();
        _blackImage = _transform.Find("BlackImage").GetComponent<Image>();
        _scoreBarUI = _transform.Find("ScoreBar").GetComponent<ScoreBarUI>();
        _scoreBarUI.Init();
        _scoreTextUI = _transform.Find("ScoreText").GetComponent<ScoreTextUI>();
        _scoreTextUI.Init();

        _whiteTransitionsImage = _transform.Find("WhiteTransitions").GetComponent<Image>();
        _blackTransitionsCanvesGroup = _transform.Find("BlackTransitions").GetComponent<CanvasGroup>();
        _guideImagesCanvesGroup = _transform.Find("GuideImagesCanves").GetComponent<CanvasGroup>();

        //_maskImage = _scoreBarUI.transform.Find("ScoreLineValue").Find("Mask").Find("OutlineMask").GetComponent<Image>();
        _maskImage = _scoreBarUI.transform.Find("ScoreLineValueMax").Find("Mask").Find("OutlineMask").GetComponent<Image>();
        InputHandler.Instance.onClickEvent += OnClick;

        _respawnCanvesGroup = _transform.Find("RespawnTimer").GetComponent<CanvasGroup>();
        _respawnText = _respawnCanvesGroup.GetComponent<TMP_Text>();

        _respawnButton = _respawnText.GetComponentInChildren<Button>();

        _respawnButton.onClick.AddListener(RespawnPlayer);
        GameManager.Instance.onLanguageChange += ChangeLanguage;

        var texts = _guideImagesCanvesGroup.GetComponentsInChildren<TMP_Text>();
        _clickText = texts[0];
        _longClickText = texts[1];
        _storeButton.onClick += Quit;

    }

    private void Quit()
    {
        GameManager.Instance.Save();
        Application.Quit();
    }

    private TMP_Text _clickText;
    private TMP_Text _longClickText;

    private void ChangeLanguage(LanguageType isChinese)
    {
        _respawnButton.GetComponentInChildren<TMP_Text>().text = GameManager.Instance.GetLanguage(LanguageKey.Revieve);
        _clickText.text = GameManager.Instance.GetLanguage(LanguageKey.QuickPress);
        _longClickText.text = GameManager.Instance.GetLanguage(LanguageKey.LongPress);
    }

    private void RespawnPlayer()
    {
        _playerManager.Respawn();
        RespawnUIClose();
        playing = false;
    }
    public void SetPlayerManager(PlayerManager manager)
    {
        if (_playerManager != null)
        {
            _playerManager.deadEvent -= DeadEventHandler;
        }
        _playerManager = manager;
        _playerManager.deadEvent += DeadEventHandler;
    }
    private void DeadEventHandler(int lifeCount)
    {
        if (lifeCount > 0)
        {
            RespawnUIOpen();
        }
        else
        {
            //ResetUI();
            StartCoroutine(DelayResetUI());
        }
    }
    public IEnumerator DelayResetUI()
    {
        yield return new WaitForSeconds(1.5f);
        ResetUI();
    }
    public void ResetUI(bool win = false)
    {
        var cm = Camera.main.GetComponent<CameraManager>();
        cm.UnLock();
        cm.transform.DOMoveY(cm.transform.position.y + 10.0f, 1.0f);
        _whiteTransitionsImage.raycastTarget = true;
        _whiteTransitionsImage.DOFade(1.0f, 1.0f).OnComplete(() =>
        {
            GameManager.Instance.NextLevel();
            _scoreTextUI.CloseCurrentScore();
            GameManager.Instance.UpdateSceneManagerAndUI();
            _playerManager.ResetState();
            _scoreBarUI.CurrentScoreBar.SetValue(0);
            _scoreBarUI.MaxScoreBar.SetValue(0);
            cm.Locked();
            _settingButton.Open();
            _storeButton.Open();
            _scoreBarUI.CurrentScoreBar.SetValue(0);
            _whiteTransitionsImage.DOFade(0.0f, 1.0f).OnComplete(() =>
            {
                _whiteTransitionsImage.raycastTarget = false;
                _guideImagesCanvesGroup.DOFade(1, 0.4f).OnComplete(() =>
                {
                    playing = false;
                });
            });

        });


    }
    private void RespawnUIClose()
    {
        _blackTransitionsCanvesGroup.interactable = false;
        _blackTransitionsCanvesGroup.blocksRaycasts = false;
        _blackTransitionsCanvesGroup.gameObject.SetActive(false);
        _blackTransitionsCanvesGroup.DOFade(0, 2.0f).OnComplete(() =>
        {
            _blackTransitionsCanvesGroup.gameObject.SetActive(true);
        });
        _respawnCanvesGroup.interactable = false;
        _respawnCanvesGroup.blocksRaycasts = false;
        _respawnCanvesGroup.alpha = 0;
    }
    private void RespawnUIOpen()
    {
        StartCoroutine(RespawnTimer());
    }
    IEnumerator RespawnTimer()
    {
        yield return new WaitForSeconds(1.5f);
        int timer = 3;
        _blackTransitionsCanvesGroup.interactable = true;
        _blackTransitionsCanvesGroup.blocksRaycasts = true;
        _blackTransitionsCanvesGroup.DOFade(1.0f, 1.4f);
        _respawnCanvesGroup.interactable = true;
        _respawnCanvesGroup.blocksRaycasts = true;
        _respawnCanvesGroup.alpha = 1;

        while (timer > -1)
        {
            _respawnText.text = timer.ToString();
            --timer;
            if (_playerManager.IsAlive())
                break;
            yield return new WaitForSeconds(1);
        }
        RespawnUIClose();
        if (!_playerManager.IsAlive())
        {
            ResetUI();
        }
    }
    public void SetColor(Color color)
    {
        _scoreBarUI.SetColor(color);

        _maskImage.color = color;
        //_maskImage2.color = color;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="score">当前得分</param>
    /// <param name="maxScore">最大得分</param>
    /// <param name="currentP">当前进度</param>
    /// <param name="scoreRequire">最大进度</param>
    /// <param name="level">关卡</param>
    public void SetScoreUI(int score, int maxScore, float currentP, float scoreRequire, int level)
    {
        ScoreTextUIView.SetCurrentScore(score);
        ScoreTextUIView.SetMaxScore(maxScore);
        ScoreBarUIView.SetLevel(level);
        ScoreBarUIView.MaxScoreBar.SetValue(scoreRequire <= 0 ? -1 : scoreRequire);
        ScoreBarUIView.CurrentScoreBar.SetValue(currentP);
    }
    public void OnClick(bool click)
    {
        if (!click)
            return;
        if (!playing)
        {
            Play();
        }
    }
    private void Play()
    {
        playing = true;
        if (_playerManager.transform.position.y >= 0)
        {
            _guideImagesCanvesGroup.DOFade(0, 0.4f);
            _settingButton.Close();
            _storeButton.Close();
        }


        _scoreTextUI.CloseMaxScore();
        _scoreTextUI.OpenCurrentScore();

        //var player = GameManager.Instance.GetMainPlayer();
        _playerManager.ActiveState();
    }
    public override UIBase Open()
    {
        _settingButton.ChangeMusic(GameManager.Instance.GetGameSave().musicSetting);
        _blackImage.color = Color.black;
        _blackImage.DOFade(0, 1.8f);
        _whiteTransitionsImage.DOFade(1, 0.8f).OnComplete(() =>
        {
            _whiteTransitionsImage.DOFade(0, 0.8f).OnComplete(() =>
            {
                _whiteTransitionsImage.raycastTarget = false;
            });
        });
        Switch(true, 0);
        _settingButton.Open();
        _storeButton.Open();


        return this;
    }
}
