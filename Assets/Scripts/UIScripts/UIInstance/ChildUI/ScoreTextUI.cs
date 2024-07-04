using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreTextUI : MonoBehaviour
{
    private CanvasGroup _scoreValueMaxCanvesGroup;
    private TMP_Text _scoreValueMaxCanvesText;
    private TMP_Text _motivationalText;
    private TMP_Text _scoreText;

    public void SetCurrentScore(int score)
    {
        _scoreText.text = score.ToString();
    }
    public void SetMaxScore(int score)
    {
        _scoreValueMaxCanvesText.text = score.ToString();
    }
    public void MotivationalEffect(int motivationalLevel)
    {

    }
    public void CloseMaxScore()
    {
        _scoreValueMaxCanvesGroup.DOFade(0, 0.4f);
    }
    public void OpenMaxScore()
    {
        _scoreValueMaxCanvesGroup.DOFade(1, 0.4f);
        _scoreText.text = "";
    }
}
