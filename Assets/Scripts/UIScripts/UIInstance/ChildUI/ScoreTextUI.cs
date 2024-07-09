using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreTextUI : MonoBehaviour
{
    private CanvasGroup _scoreValueMaxCanvesGroup;
    private RectTransform _motivationRectTrans;
    private TMP_Text _scoreValueMaxCanvesText;
    private TMP_Text _motivationalText;
    private TMP_Text _scoreText;

    public void Init()
    {
        Transform _trans = transform;
        _scoreValueMaxCanvesText = _trans.Find("ScoreValueMax").GetComponent<TMP_Text>();
        _scoreValueMaxCanvesGroup = _scoreValueMaxCanvesText.GetComponent<CanvasGroup>();
        _motivationalText = _trans.Find("MotivationalText").GetComponent<TMP_Text>();
        _motivationRectTrans = _motivationalText.GetComponent<RectTransform>();
        _scoreText = _trans.Find("ScoreText").GetComponent<TMP_Text>();
        GameManager.Instance.onMotivational += MotivationalEffect;
    }
    public void SetCurrentScore(int score)
    {
        _scoreText.text = score.ToString();
    }
    public void SetMaxScore(int score)
    {
        _scoreValueMaxCanvesText.text = score.ToString();
    }

    public void MotivationalEffect(int motivationalLevel)
    {
        if (!GameManager.Instance.GetGameSave().motivationalSetting)
            return;

        if (motivationalLevel > 1)
        {
            //TODO:µ¯³öÏÔÊ¾
            _motivationalText.text = GameManager.Instance.GetLanguage(LanguageKey.MotivationValue_1 + motivationalLevel - 2);
            _motivationRectTrans.localScale = Vector3.one * 0.4f;
            _motivationalText.rectTransform.DOScale(Vector3.one * 1.2f, 0.24f).OnComplete(() =>
            {
                _motivationalText.rectTransform.DOScale(Vector3.one, 0.1f);
            });
        }
        else
        {
            _motivationalText.text = "";
        }
    }
    public void CloseMaxScore()
    {
        _scoreValueMaxCanvesGroup.DOFade(0, 0.4f);
    }
    public void OpenMaxScore()
    {
        _scoreValueMaxCanvesGroup.DOFade(1, 0.4f);
    }
    public void CloseCurrentScore()
    {
        _scoreText.alpha = 0;
    }
    public void OpenCurrentScore()
    {
        SetCurrentScore(0);
        _scoreText.DOFade(1, 0.4f);
    }
}
