using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelConfig
{
    //关卡主色
    public Color color;
    //难度(float)
    public float diffcult;
    //过关需求
    public int levelScoreRequire;
    //关卡长度
    public float levelLength;
    public GameObject[] scenesEntitys;
    public int addScore = 30;
    public float addTimer = 0.3f;
}
