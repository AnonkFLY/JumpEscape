using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestUI : MonoBehaviour
{
    private TMP_Text _text;
    private Button _button;
    private GameObject _root;
    private bool _show = false;
    public void Init()
    {
        _root = transform.GetChild(0).gameObject;
        _text = _root.GetComponentInChildren<TMP_Text>();
        _button = GetComponentInChildren<Button>();
        _button.onClick.AddListener(Call);
        Application.logMessageReceived += handler;
    }

    private void handler(string condition, string stackTrace, LogType type)
    {
        _text.text += $"{type}:{condition}\n>>>{stackTrace}\n";
    }

    private void Call()
    {
        _show = !_show;
        _root.SetActive(_show);
    }
}
