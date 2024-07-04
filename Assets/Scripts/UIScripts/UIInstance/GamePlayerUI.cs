using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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
    public override void Init()
    {
        _settingButton = transform.Find("SettingViewport").GetComponent<SettingUI>();
        _storeButton = transform.Find("StoreViewport").GetComponent<SettingUI>();
        _blackImage = _transform.Find("BlackImage").GetComponent<Image>();
        _scoreBarUI = _transform.Find("ScoreBar").GetComponent<ScoreBarUI>();
        _scoreTextUI = _transform.Find("ScoreText").GetComponent<ScoreTextUI>();

        _whiteTransitionsImage = _transform.Find("WhiteTransitions").GetComponent<Image>();
        _blackTransitionsCanvesGroup = _transform.Find("BlackTransitions").GetComponent<CanvasGroup>();
        _guideImagesCanvesGroup = _transform.Find("GuideImagesCanves").GetComponent<CanvasGroup>();
        InputHandler.Instance.onClickEvent += OnClick;
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
        _guideImagesCanvesGroup.DOFade(0, 0.4f);
        var player = GameManager.Instance.GetMainPlayer();
        player.ActiveState();
    }
    public override UIBase Open()
    {
        _blackImage.color = Color.black;
        _blackImage.DOFade(0, 1.8f);
        Switch(true, 0);
        _settingButton.Open();
        //_storeButton.Open();
        return this;
    }
}
