using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class UIManager
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UIManager();
            }
            return _instance;
        }

        set => _instance = value;
    }
    private Transform _canveUI;
    /// <summary>
    /// UI堆栈
    /// </summary>
    private List<UIBase> _uiList = new List<UIBase>();
    private Dictionary<string, UIBase> _instanceUI = new Dictionary<string, UIBase>();
    private UIAssets _uiAssets;
    /// <summary>
    /// 加载所有ui基本数据
    /// </summary>
    public void LoadUIResource(UIAssets uIAssets)
    {
        _uiAssets = uIAssets;
        _canveUI = GameObject.Find("UICanves").transform;
    }
    public UIBase PreLoadUI(string type)
    {
        var panel = GetUI(type);
        panel.Hide();
        return panel;
    }
    public UIBase Open(string type, bool closeLast = false)
    {
        var panel = GetUI(type);
        if (closeLast)
            Close();
        if (panel.IsOpen)
            return panel;
        panel.Open();
        _uiList.Add(panel);
        return panel;
    }
    public void Close()
    {
        if (_uiList.Count > 0)
            Close(_uiList.Count - 1);
    }
    public UIBase Close(int index)
    {
        if (index < 0 || index >= _uiList.Count)
        {
            Debug.LogError($"Index Error {index} / {_uiList.Count}");
            return null;
        }
        var panel = _uiList[index];
        if (!panel.IsOpen)
            return panel;
        panel.Close();
        _uiList.RemoveAt(index);
        return panel;
    }
    private UIBase GetUI(string type)
    {
        UIBase panel;
        if (!_instanceUI.TryGetValue(type, out panel))
        {
            panel = GameObject.Instantiate(_uiAssets.GetUIObj(type), _canveUI).GetComponent<UIBase>();
            _instanceUI.Add(type, panel);
        }
        return panel;
    }
    public T GetUI<T>(string type) where T : UIBase
    {
        var panel = GetUI(type);
        return panel as T;
    }
}
