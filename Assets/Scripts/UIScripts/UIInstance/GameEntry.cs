using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEntry : MonoBehaviour
{

    //Animator event
    public void StartOver()
    {
        UIManager.Instance.Open("GamePlayerUI");
        Destroy(gameObject);
    }
    //public override UIBase Open()
    //{
    //    base.Open();
    //    return this;
    //}
    //public override UIBase Close()
    //{
    //    UIManager.Instance.Open("StartUI");
    //    //ֻ��Ҫ����һ�� �رպ�����
    //    Destroy(gameObject);
    //    return null;
    //}
}
