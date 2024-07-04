using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[SerializeField]
public class BodyController
{
    //private SpriteRenderer _brimSpriteR;
    private SpriteRenderer _hatSpriteR;
    private SpriteRenderer _headSpriteR;
    private SpriteRenderer _faceSpriteR;
    private SpriteRenderer _bootSpriteR;
    private Transform _bootTransSpriteR;
    private Sprite[] _faceStyleSprites;
    public void Init(Transform bodyTrans, Sprite[] faceStyleSprites)
    {
        _faceStyleSprites = faceStyleSprites;
        //_brimSpriteR = bodyTrans.Find("Brim").GetComponent<SpriteRenderer>();
        _hatSpriteR = bodyTrans.Find("Hat").GetComponent<SpriteRenderer>();
        _headSpriteR = bodyTrans.Find("Head").GetComponent<SpriteRenderer>();
        _faceSpriteR = bodyTrans.Find("Face").GetComponent<SpriteRenderer>();
        _bootTransSpriteR = bodyTrans.Find("BootCenter");
        _bootSpriteR = _bootTransSpriteR.GetComponentInChildren<SpriteRenderer>();
    }
    //body color
    public void SetMainColor(Color color)
    {
        _headSpriteR.color = color;
        _faceSpriteR.color = color;
        _bootSpriteR.color = color;
    }
    //hat color
    public void SetSecondColor(Color color)
    {
        _hatSpriteR.color = color;
    }
    public void SetFaceStyle(int style)
    {
        if (style < 0 || style > _faceStyleSprites.Length)
        {
            Debug.LogWarning($"{style}:Exceeded index limit");
            return;
        }
        _faceSpriteR.sprite = _faceStyleSprites[style];
    }
    public void ChangeLeg()
    {
        Vector3 currentRotation = _bootTransSpriteR.eulerAngles;
        currentRotation.y += 180;
        _bootTransSpriteR.eulerAngles = currentRotation;
    }
}

public class PlayerManager : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private BodyController _bodyController = new BodyController();
    [Header("Paramaters")]
    [SerializeField] private int maxPlayerLife = 2;
    [SerializeField] private int currentPlayerLife = 2;
    [SerializeField] private float downSpeed = 10.0f;
    [SerializeField] private float addSpeed = 2.0f;
    [SerializeField] private Vector3 currentSpeed;
    [Header("FaceSprites")]
    [SerializeField] private Sprite[] _faceStyleSprites;
    private Transform _transform;
    private Animator _animator;
    private GameObject _bodyGameObject;
    //判断是否在滑动的变量
    private bool activeState = false;
    private bool isLive = true;
    private bool isClickState = false;




    public event UnityAction<int> deadEvent;
    private void Awake()
    {
        _transform = transform;
        _animator = GetComponent<Animator>();
        _lineRenderer = GetComponentInChildren<LineRenderer>();
        _bodyGameObject = _transform.Find("Body").gameObject;
        _bodyController.Init(_bodyGameObject.transform, _faceStyleSprites);
        currentSpeed.y = -downSpeed;
        currentSpeed.x = downSpeed * 0.75f;
        StopState();
    }
    int i = 0;
    private void Update()
    {
        if (isLive && activeState)
        {
            _transform.Translate(currentSpeed * Time.deltaTime);
        }
    }
    //重置状态回到起点
    public void ResetState()
    {
        _transform.position = Vector3.zero;
        currentPlayerLife = maxPlayerLife;
        isLive = true;
        StopState();
    }
    //激活状态->开滑！
    public void ActiveState()
    {
        _animator.enabled = true;
        activeState = true;
    }
    //停止状态
    public void StopState()
    {
        _animator.enabled = false;
        activeState = false;
    }
    //复活在原地
    public void Respawn()
    {
        _bodyGameObject.SetActive(true);
        Vector3 pos = _transform.position;
        pos.x = 0;
        _transform.position = pos;
    }
    //飘逸:增加纵向速度
    public void Drift()
    {

    }
    //转弯：改变方向
    public void Turning()
    {
        _bodyController.ChangeLeg();
        currentSpeed.x = -currentSpeed.x;
    }
    //死亡
    public void PlayerDead()
    {
        //TODO:播放粒子效果

        //消失
        _bodyGameObject.SetActive(false);
        --currentPlayerLife;
        if (currentPlayerLife == 0)
            isLive = false;
        deadEvent?.Invoke(currentPlayerLife);
    }
    public void OnClick(bool onClick)
    {
        if (!isLive || !activeState)
            return;
        isClickState = onClick;
        if (onClick)
        {
            Turning();
        }
    }
}
