using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private static InputHandler _instance;
    public static InputHandler Instance { get => _instance; }
    public bool OnClick { get => onClick; }

    private bool onClick = false;
    private Image _image;

    public event UnityAction<bool> onClickEvent;
    private void Awake()
    {
        _image = GetComponent<Image>();
        SingleInit();
    }
    private void SingleInit()
    {
        if (_instance == null)
            _instance = this;
        if (_instance != this)
            Destroy(gameObject);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        onClick = true;
        //_image.color = Color.red;
        onClickEvent?.Invoke(onClick);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        onClick = false;
        //_image.color = Color.white;
        onClickEvent?.Invoke(onClick);
    }
}
