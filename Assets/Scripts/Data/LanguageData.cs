using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum LanguageKey
{
    QuickPress,
    LongPress,
    Revieve,
    LevelComplate,
    CrazyMode,
    NormalMode,
    MotivationValue_1,
    MotivationValue_2,
    MotivationValue_3,
    MotivationValue_4,
    MotivationValue_5,
    MotivationValue_6,
    MotivationValue_7,
    MotivationValue_8,
    MotivationValue_9,
    MotivationValue_10,
    MotivationValue_11,
    MotivationValue_12,
    MotivationValue_13,
    MotivationValue_14,
    MotivationValue_15,
}
public enum LanguageType
{
    English,
    Chinese,
}
[Serializable]
public struct LanguageData
{
    public List<string> datas;
    public LanguageType type;
}
