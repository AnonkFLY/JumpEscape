using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ButtonState
{
    public bool isPressed = true;
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
    //��ǰ�ؿ�
    public int level = 1;
    //��ʷ����
    public int currentLevelMaxScore = 0;
    public float currentLevelMaxScorePer = 0.0f;

}
