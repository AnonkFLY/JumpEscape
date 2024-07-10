using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MotivationalManager
{
    //rating效果只持续3s
    private readonly float ratingEffectTimer = 5.5f;
    public float ratingTimer = 0.0f;
    //得分
    public int currentRating = 1;
    public int currentScore = 0;
    public int scoreRequire = 0;

    /// <summary>
    /// add score
    /// </summary>
    /// <param name="score">score value</param>
    /// <param name="scoreLevel">score level,level 0 not add rating</param>
    public int AddScore(int score, int scoreLevel = 0)
    {
        if (scoreLevel == 0)
        {
            currentScore += score;
            return currentRating;
        }
        ++currentRating;
        if (currentRating > 16)
        {
            currentRating = 16;
        }

        ratingTimer = ratingEffectTimer;
        currentScore += score * currentRating;
        //Debug.Log("Add score " + score * currentRating);

        return currentRating;
    }
    public float GetPercentage()
    {
        return (float)currentScore / (float)scoreRequire;
    }
}
