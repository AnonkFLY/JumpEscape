using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public abstract class UIBase : MonoBehaviour
{
    [SerializeField]
    private string _uiName;
    private bool _isOpen = false;
    public bool IsOpen { get => _isOpen; }
    public string UIName { get => _uiName; }

    protected Transform _transform;
    protected CanvasGroup _canvasGroup;
    [SerializeField] private float _fadeTime = .2f;
    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        Init();
    }
    public void Hide()
    {
        Switch(false, 0);
    }
    public virtual void Switch(bool open, float fadeTime)
    {
        _isOpen = open;
        if (_fadeTime > 0)
            _canvasGroup.DOFade(open ? 1.0f : 0.0f, fadeTime);
        else
            _canvasGroup.alpha = open ? 1.0f : 0.0f;
        _canvasGroup.interactable = open;
        _canvasGroup.blocksRaycasts = open;
    }
    public virtual UIBase Open()
    {
        Switch(true, _fadeTime);
        return this;
    }

    public virtual UIBase Close()
    {
        Switch(false, _fadeTime);
        return this;
    }
    public virtual void Init() { }

}
