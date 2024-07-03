using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public class SettingButton
{
    private Button _button;
    private Image _back;
    private Image _icon;
    [SerializeField] private Sprite[] _sprite;
    [SerializeField] private Sprite[] _iconSprite;
    private ButtonState _state;
    public RectTransform _rectTransform;
    Vector2 _originPosition;
    public void Init(string name, Transform transform, ButtonState state)
    {
        _button = transform.Find(name).GetComponent<Button>();
        _rectTransform = _button.GetComponent<RectTransform>();
        _back = _button.GetComponent<Image>();
        _icon = _button.transform.GetChild(0).GetComponent<Image>();
        _state = state;
        SetState(_state.isPressed);
        _button.onClick.AddListener(ButtonEvent);
        _originPosition = _rectTransform.localPosition;
        _rectTransform.localPosition = Vector2.zero;
    }
    public void SetPosState(bool isPressed)
    {
        _rectTransform.DOLocalMove(isPressed ? _originPosition : Vector2.zero, isPressed ? 0.15f : 0.08f);
    }
    private void ButtonEvent()
    {
        SetState(!_state.isPressed);
    }
    private void SetState(bool state)
    {
        int index = state ? 0 : 1;
        _back.sprite = _sprite[index];
        _icon.sprite = _iconSprite[index];
        _state.isPressed = state;
        GameManager.Instance.Save();
    }
}

public class SettingUI : UIBase
{
    [SerializeField] private SettingButton _soundButton;
    [SerializeField] private SettingButton _motivationalButton;
    //private Button settingButton;
    private RectTransform _rectTransforom;
    [Header("Animation")]
    [SerializeField] private float effectScale = 1.2f;
    [SerializeField] private float effectTimer = 1.0f;

    private bool _listOpen = false;
    private void Start()
    {
        _transform.Find("SettingButton").GetComponent<Button>().onClick.AddListener(SwitchList);
        GameSave gameSave = GameManager.Instance.GetSave();
        _soundButton.Init("SoundButton", _transform, gameSave.musicSetting);
        _motivationalButton.Init("MotivationalButton", _transform, gameSave.motivationalSetting);

    }
    private void SwitchList()
    {
        _listOpen = !_listOpen;
        _motivationalButton.SetPosState(_listOpen);
        _soundButton.SetPosState(_listOpen);
    }
    public override void Switch(bool open, float fadeTime)
    {
        if (!_rectTransforom)
            _rectTransforom = transform.GetComponent<RectTransform>();
        _rectTransforom.localScale = open ? Vector3.zero : Vector3.one;
        float timer1 = open ? effectTimer * .8f : effectTimer * .2f;
        float timer2 = open ? effectTimer * .2f : effectTimer * .8f;

        _rectTransforom.DOScale(effectScale, timer1).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            _rectTransforom.DOScale(open ? Vector3.one : Vector3.zero, timer2).SetEase(Ease.InQuad);

        });
    }
}
