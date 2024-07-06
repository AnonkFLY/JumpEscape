using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelConfig
{
    //�ؿ���ɫ
    public Color color;
    //�Ѷ�(float)
    public float diffcult;
    //��������
    public int levelScoreRequire;
    //�ؿ�����
    public float levelLength;
    public GameObject[] scenesEntitys;
    public int addScore = 30;
    public float addTimer = 0.3f;
}
