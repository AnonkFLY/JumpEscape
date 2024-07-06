using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum DamageType
{
    SceneDamage,
    EntityDamage
}

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
    public void ChangeLeg(int direction)
    {
        Vector3 currentRotation = _bootTransSpriteR.eulerAngles;
        currentRotation.y = 180 * (direction == 1 ? 0 : 1);
        _bootTransSpriteR.eulerAngles = currentRotation;
    }
}

public class PlayerManager : MonoBehaviour
{
    private TrailRenderer _trailRenderer;
    private BodyController _bodyController = new BodyController();
    [Header("Paramaters")]
    [SerializeField] private int maxPlayerLife = 2;
    [SerializeField] private int currentPlayerLife = 2;
    [SerializeField] private int maxArmor = 0;
    [SerializeField] private int currentArmor = 0;
    //下降速度
    [SerializeField] private float downSpeed = 10.0f;
    //左右正常最大速度
    [SerializeField] private float driftSpeed = 7.5f;
    //加速后最大速度
    [SerializeField] private float addMaxSpeed = 12.0f;
    //加速度
    [SerializeField] private float addSpeed = 20f;
    //转向插值
    [SerializeField] private float changeSpeed = 2.0f;

    //当前最大速度
    private float currentMaxSpeed;
    private Vector3 currentSpeed;

    [Header("FaceSprites")]
    [SerializeField] private Sprite[] _faceStyleSprites;
    private Transform _transform;
    private Animator _animator;
    private GameObject _bodyGameObject;
    //判断是否在滑动的变量
    private bool _activeState = false;
    //是否存活
    private bool _isLive = true;
    private bool _isClickState = false;

    private int _direction = -1;



    public event UnityAction<int> deadEvent;
    private void Awake()
    {
        _transform = transform;
        _animator = GetComponent<Animator>();
        _trailRenderer = GetComponentInChildren<TrailRenderer>();
        _bodyGameObject = _transform.Find("Body").gameObject;
        _bodyController.Init(_bodyGameObject.transform, _faceStyleSprites);

        ResetState();
    }
    private void SpeedCorrection()
    {
        if (currentSpeed.x * _direction < 0)
        {
            currentSpeed.x = Mathf.Lerp(currentSpeed.x, currentMaxSpeed * _direction, changeSpeed * Time.deltaTime);
        }
        else
        {
            currentSpeed.x += (_isClickState ? addSpeed * 1.2f : addSpeed) * Time.deltaTime * _direction;
        }

        if (_direction == 1 && currentSpeed.x >= currentMaxSpeed)
        {
            currentSpeed.x = currentMaxSpeed;
        }
        else if (_direction == -1 && currentSpeed.x <= -currentMaxSpeed)
        {
            currentSpeed.x = -currentMaxSpeed;
        }
    }
    private void Update()
    {
        if (_isLive && _activeState)
        {
            currentMaxSpeed = _isClickState ? addMaxSpeed : driftSpeed;
            SpeedCorrection();
            _transform.Translate(currentSpeed * Time.deltaTime);
        }
    }
    public bool IsAlive()
    {
        return _isLive;
    }
    public bool IsActive()
    {
        return _activeState;
    }
    //重置状态回到起点
    public void ResetState()
    {
        _bodyGameObject.SetActive(true);
        _direction = -1;
        _transform.position = Vector3.zero;
        currentPlayerLife = maxPlayerLife;
        currentArmor = maxArmor;
        _isLive = true;
        _activeState = false;
        _trailRenderer.Clear();
        currentSpeed.y = -downSpeed;
        currentSpeed.x = 0;
        currentMaxSpeed = driftSpeed;
        StopState();
    }
    //激活状态->开滑！
    public void ActiveState()
    {
        _animator.enabled = true;
        _activeState = true;
        _direction = -1;
    }
    //停止状态
    public void StopState()
    {
        _animator.enabled = false;
        _activeState = false;
    }
    //复活在原地
    public void Respawn()
    {
        _bodyGameObject.SetActive(true);
        Vector3 pos = _transform.position;
        pos.x = 0;
        _transform.position = pos;
        _activeState = false;
        _isLive = true;
        _trailRenderer.Clear();
        _direction = -1;
    }
    ////飘逸:增加纵向速度
    //public void Drift()
    //{
    //    currentMaxSpeed = addMaxSpeed;
    //}
    //转弯：改变方向
    public void Turning()
    {
        _direction = -_direction;
        _bodyController.ChangeLeg(_direction);
    }
    /// <summary>
    /// 受伤
    /// </summary>
    /// <param name="hurtType">场景伤害不可被护盾减免</param>
    public void PlayerHurt(DamageType hurtType = DamageType.EntityDamage, GameObject damageOrigin = null)
    {
        Debug.Log("DEAD");

        if (currentArmor > 0 && hurtType == DamageType.EntityDamage)
        {
            --currentArmor;
            damageOrigin?.GetComponent<ICanDestroy>()?.Destroy();
            return;
        }
        //TODO:播放粒子效果

        //消失
        _bodyGameObject.SetActive(false);
        --currentPlayerLife;
        _isLive = false;
        _activeState = false;

        if (currentPlayerLife <= 0)
        {
            GameManager.Instance.ResetScore();
        }

        deadEvent?.Invoke(currentPlayerLife);
    }
    public void OnClick(bool onClick)
    {
        if (!_isLive || !_activeState)
            return;
        _isClickState = onClick;
        if (onClick)
        {
            Turning();
        }
    }
}
