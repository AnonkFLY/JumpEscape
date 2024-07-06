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

    public void Init()
    {
        Transform _trans = transform;
        _scoreValueMaxCanvesText = _trans.Find("ScoreValueMax").GetComponent<TMP_Text>();
        _scoreValueMaxCanvesGroup = _scoreValueMaxCanvesText.GetComponent<CanvasGroup>();
        _motivationalText = _trans.Find("MotivationalText").GetComponent<TMP_Text>();
        _scoreText = _trans.Find("ScoreText").GetComponent<TMP_Text>();
    }
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
    }
    public void CloseCurrentScore()
    {
        _scoreText.DOFade(0, 0.4f);
    }
    public void OpenCurrentScore()
    {
        SetCurrentScore(0);
        _scoreText.DOFade(1, 0.4f);
    }
}
