using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MotivationalManager
{
    //rating效果只持续3s
    private readonly float ratingTimer = 3f;
    //得分
    public int currentRating = 0;
    public int currentScore = 0;
    public int scoreRequire = 0;

    /// <summary>
    /// add score
    /// </summary>
    /// <param name="score">score value</param>
    /// <param name="scoreLevel">score level,level 0 not add rating</param>
    public void AddScore(int score, int scoreLevel = 0)
    {
        if (scoreLevel == 0)
        {
            currentScore += score;
            return;
        }
    }
    public float GetPercentage()
    {
        return (float)currentScore / (float)scoreRequire;
    }
}
