using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ButtonState
{
    public bool isPressed = true;
    public static implicit operator bool(ButtonState obj)
    {
        return obj.isPressed;
    }

    public static implicit operator ButtonState(bool v)
    {
        return new ButtonState() { isPressed = v };
    }
}
[Serializable]
public class GameSave
{
    const int roleCount = 4;
    public ButtonState musicSetting = new ButtonState();
    public ButtonState motivationalSetting = new ButtonState();
    //lock roles
    public bool[] roles = new bool[roleCount] { true, false, false, false };
    public int selectRole = 0;
    //当前关卡
    public int level = 1;
    //历史分数
    public int currentLevelMaxScore = 0;
    public float currentLevelMaxScorePer = 0.0f;
    public Color GetCurrentRandomColor = Color.white;
    public ButtonState isChinese = new ButtonState();
}
