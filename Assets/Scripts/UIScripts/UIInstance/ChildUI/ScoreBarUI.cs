using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ScoreBarValue
{
    private float _value = 0.0f;
    private RectTransform _rectTransform;
    private RectTransform _tagRectTransform;
    private Image _barImage;
    private Image _tagImage;
    private Image _TagTrImage;
    private Image _maskImage;
    private TMP_Text _text;
    private Vector2 _originPos;
    private CanvasGroup _tagCanvas;
    private Vector2 _tempValue;
    public void Init(RectTransform rectTransform)
    {
        _rectTransform = rectTransform;
        _barImage = _rectTransform.GetComponent<Image>();
        _tagRectTransform = _rectTransform.Find("ValueTag").GetComponent<RectTransform>();
        _maskImage = _rectTransform.Find("Mask").Find("OutlineMask").GetComponent<Image>(); ;
        _tagCanvas = _tagRectTransform.GetComponent<CanvasGroup>();
        _tagImage = _tagRectTransform.Find("Rect").GetComponent<Image>();
        _TagTrImage = _tagRectTransform.Find("TrImage").GetComponent<Image>();
        _originPos = _tagRectTransform.localPosition;
        _text = _tagRectTransform.Find("TextValue").GetComponent<TMP_Text>();

        _text.text = "0%";

        _tempValue = Vector2.zero;
    }
    public void CloseTag()
    {
        if (_tagCanvas)
            _tagCanvas.alpha = 0;
    }
    public void ShowTag()
    {
        if (_tagCanvas)
            _tagCanvas.alpha = 1;
    }
    public void SetValue(float value)
    {
        if (value < 0)
        {
            CloseTag();
            return;
        }
        else
        {
            ShowTag();
        }
        _value = value;
        int valueInt = (int)(value * 100.0f);
        _barImage.fillAmount = value;
        _tempValue.x = _rectTransform.rect.width * value;
        _tagRectTransform.localPosition = _originPos + _tempValue;

        //Debug.Log($"{value}->{_originPos}[{_tempValue}/{_rectTransform.rect.x}]=>{_tagRectTransform.localPosition}");
        _text.text = $"{valueInt}%";
    }
    public void SetColor(Color color)
    {
        _barImage.color = color;
        _tagImage.color = color;
        _TagTrImage.color = color;
        _maskImage.color = color;
    }
}

public class ScoreBarUI : MonoBehaviour
{
    private ScoreBarValue _currentScoreBar = new ScoreBarValue();
    private ScoreBarValue _maxScoreBar = new ScoreBarValue();
    private Transform _transform;

    private Image _levelHeadImage;
    private Image _scoreLineBackImage;
    private Image _nextLevelTailOutline;
    private TMP_Text _nextLevelText;
    private TMP_Text _currentLevelText;

    public ScoreBarValue MaxScoreBar { get => _maxScoreBar; }
    public ScoreBarValue CurrentScoreBar { get => _currentScoreBar; }

    public void Init()
    {
        _transform = transform;
        _currentScoreBar.Init(_transform.Find("ScoreLineValue").GetComponent<RectTransform>());
        _maxScoreBar.Init(_transform.Find("ScoreLineValueMax").GetComponent<RectTransform>());

        //LevelHead,ScoreLineBack,ShowLevelTail(NextLevelTailOutline,NextLevelText)
        _levelHeadImage = _transform.Find("LevelHead").GetComponent<Image>();
        _scoreLineBackImage = _transform.Find("ScoreLineBack").GetComponent<Image>();
        Transform levelTailTrans = _transform.Find("ShowLevelTail");
        _nextLevelTailOutline = levelTailTrans.Find("NextLevelTailOutline").GetComponent<Image>();
        _nextLevelText = levelTailTrans.Find("NextLevelText").GetComponent<TMP_Text>();

        _currentLevelText = _levelHeadImage.GetComponentInChildren<TMP_Text>();

    }

    public void SetLevel(int level)
    {
        _currentLevelText.text = level.ToString();
        _nextLevelText.text = (level + 1).ToString();
    }
    public void SetColor(Color color)
    {
        _currentScoreBar.SetColor(color);
        //_maxScoreBar.SetColor(color);
        _levelHeadImage.color = color;
        _scoreLineBackImage.color = color;
        _nextLevelTailOutline.color = color;
        _nextLevelText.color = color;
    }
}
