using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEntry : MonoBehaviour
{

    private void Start()
    {
        //
        UIManager.Instance.PreLoadUI("GamePlayerUI");
        
    }
    //TODO:播放动画,结束后销毁
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
    //    //只需要加载一次 关闭后销毁
    //    Destroy(gameObject);
    //    return null;
    //}
}
