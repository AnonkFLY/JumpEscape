using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private ParticleSystem _deathEffect;
    private ParticleSystem _footPrint;
    private ParticleSystem _trailEffect;
    private ParticleSystem _footPrintTurn;
    public void Init(Transform bodyTrans, Sprite[] faceStyleSprites, Transform root)
    {
        _faceStyleSprites = faceStyleSprites;
        //_brimSpriteR = bodyTrans.Find("Brim").GetComponent<SpriteRenderer>();
        _hatSpriteR = bodyTrans.Find("Hat").GetComponent<SpriteRenderer>();
        _headSpriteR = bodyTrans.Find("Head").GetComponent<SpriteRenderer>();
        _faceSpriteR = bodyTrans.Find("Face").GetComponent<SpriteRenderer>();
        _bootTransSpriteR = bodyTrans.Find("BootCenter");
        _bootSpriteR = _bootTransSpriteR.GetComponentInChildren<SpriteRenderer>();
        _deathEffect = root.Find("DeathEffect").GetComponent<ParticleSystem>();
        _footPrint = root.Find("FootPrint").GetComponent<ParticleSystem>();
        _trailEffect = root.Find("TrailEffect").GetComponent<ParticleSystem>();
        _footPrintTurn = _bootTransSpriteR.GetComponentInChildren<ParticleSystem>();
    }
    //body color
    public void SetMainColor(Color color)
    {
        _headSpriteR.color = color;
        //_faceSpriteR.color = color;
        _bootSpriteR.color = color;

        _deathEffect.startColor = color;
        _trailEffect.startColor = color;
        _footPrint.startColor = color;
    }
    public void TurningEffect(bool open)
    {
        if (open)
            _footPrintTurn.Play();
        else _footPrintTurn.Stop();
    }
    public void FootEffect(bool open)
    {
        if (open)
        {
            _footPrint.Play();
            _trailEffect.Play();
        }
        else
        {
            _footPrint.Stop();
            _trailEffect.Stop();
        }
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
    public void DeathEffect()
    {
        _deathEffect.Play();
    }

    public void NoramlFace()
    {
        _faceSpriteR.sprite = _faceStyleSprites[0];
    }

    public void FireFace()
    {
        _faceSpriteR.sprite = _faceStyleSprites[1];
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
    //�½��ٶ�
    [SerializeField] private float downSpeed = 10.0f;
    //������������ٶ�
    [SerializeField] private float speed = 7.5f;
    //���ٺ�����ٶ�
    [SerializeField] private float addMaxSpeed = 12.0f;
    //���ٶ�
    [SerializeField] private float addSpeed = 20f;
    //ת���ֵ
    [SerializeField] private float changeSpeed = 2.0f;

    //��ǰ���������ٶ�
    private float _currentMaxSpeed;
    private Vector3 _currentSpeed;

    [Header("FaceSprites")]
    [SerializeField] private Sprite[] _faceStyleSprites;
    private Transform _transform;
    private Animator _animator;
    private GameObject _bodyGameObject;
    //�ж��Ƿ��ڻ����ı���
    private bool _activeState = false;
    //�Ƿ���
    private bool _isLive = true;
    private bool _isClickState = false;
    public bool winState = false;

    private int _direction = -1;
    private AudioSource _audioSource;



    public event UnityAction<int> deadEvent;
    private void Awake()
    {
        _transform = transform;
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _audioSource.Play();
        _audioSource.Pause();
        _trailRenderer = GetComponentInChildren<TrailRenderer>();
        _bodyGameObject = _transform.Find("Body").gameObject;
        _bodyController.Init(_bodyGameObject.transform, _faceStyleSprites, _transform);

        ResetState();
    }
    public BodyController GetBodyController() { return _bodyController; }
    public void SetTrailColor(Color color)
    {
        _trailRenderer.startColor = color;
    }
    private void SpeedCorrection()
    {
        if (_currentSpeed.x * _direction < 0)
        {
            _currentSpeed.x = Mathf.Lerp(_currentSpeed.x, _currentMaxSpeed * _direction, changeSpeed * Time.deltaTime);
        }
        else
        {
            _currentSpeed.x += (_isClickState ? addSpeed * 1.2f : addSpeed) * Time.deltaTime * _direction;
        }

        if (_direction == 1 && _currentSpeed.x >= _currentMaxSpeed)
        {
            _currentSpeed.x = _currentMaxSpeed;
        }
        else if (_direction == -1 && _currentSpeed.x <= -_currentMaxSpeed)
        {
            _currentSpeed.x = -_currentMaxSpeed;
        }
    }
    private void Update()
    {
        if (_isLive && _activeState)
        {
            _currentMaxSpeed = _isClickState ? addMaxSpeed : speed;
            SpeedCorrection();
            _transform.Translate(_currentSpeed * Time.deltaTime);
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
    //����״̬�ص����
    public void ResetState()
    {
        Camera.main.GetComponent<CameraManager>().Locked();
        _bodyGameObject.SetActive(true);
        _direction = -1;
        _transform.position = Vector3.zero;
        currentPlayerLife = maxPlayerLife;
        currentArmor = maxArmor;
        _isLive = true;
        _activeState = false;
        winState = false;
        _trailRenderer.Clear();
        _currentSpeed.y = -downSpeed;
        _currentSpeed.x = 0;
        _currentMaxSpeed = speed;
        OnMotivational(1);
        StopState();
    }
    //����״̬->������
    public void ActiveState()
    {
        _animator.enabled = true;
        _activeState = true;
        _direction = -1;
    }
    //ֹͣ״̬
    public void StopState()
    {
        _animator.enabled = false;
        _activeState = false;
        _bodyController.TurningEffect(false);
        OnMotivational(1);
    }
    //������ԭ��
    public void Respawn()
    {
        _bodyGameObject.SetActive(true);
        Vector3 pos = _transform.position;
        _currentSpeed.x = 0;
        pos.x = 0;
        _transform.position = pos;
        _activeState = false;
        _isLive = true;
        _trailRenderer.Clear();
        _direction = -1;
    }
    ////Ʈ��:���������ٶ�
    //public void Drift()
    //{
    //    currentMaxSpeed = addMaxSpeed;
    //}
    //ת�䣺�ı䷽��
    public void Turning(bool onclick)
    {
        if (onclick)
        {
            _direction = -_direction;
            _bodyController.ChangeLeg(_direction);
            _bodyController.TurningEffect(true);
            if (!winState && GameManager.Instance.GetSave().musicSetting)
                _audioSource.UnPause();
        }
        else
        {
            _bodyController.TurningEffect(false);
            _audioSource.Pause();
        }

    }
    /// <summary>
    /// ����
    /// </summary>
    /// <param name="hurtType">�����˺����ɱ����ܼ���</param>
    public void PlayerHurt(DamageType hurtType = DamageType.EntityDamage, GameObject damageOrigin = null)
    {
        if (winState || GameManager.Instance.unconqueredState || !IsActive())
            return;

        if (currentArmor > 0 && hurtType == DamageType.EntityDamage)
        {
            --currentArmor;
            damageOrigin?.GetComponent<ICanDestroy>()?.Destroy();
            return;
        }
        //TODO:��������Ч��
        _bodyController.DeathEffect();
        Turning(false);
        AudioManager.Instance.PlaySoundEffect(0);
        //��ʧ
        _bodyGameObject.SetActive(false);
        --currentPlayerLife;
        _isLive = false;
        _activeState = false;
        GameManager.Instance.onMotivational(1);

        //if (currentPlayerLife <= 0)
        //{
        //    GameManager.Instance.ResetScore();
        //}

        deadEvent?.Invoke(currentPlayerLife);
    }
    public void OnClick(bool onClick)
    {
        if (!_isLive || !_activeState || winState)
            return;

        _isClickState = onClick;

        Turning(onClick);
    }

    public void OnMotivational(int value)
    {
        if (value <= 1)
        {
            _bodyController.SetMainColor(GameManager.Instance.GetCurrentLevelColor());
            SetTrailColor(GameManager.Instance.GetCurrentLevelColor());
            _bodyController.FootEffect(false);
            _bodyController.NoramlFace();
        }
        else if (value >= 3)
        {
            if (value == 3)
            {
                _bodyController.FireFace();
                AudioManager.Instance.PlaySoundEffect(3);
                //Debug.Log("player ");
            }
            _bodyController.SetMainColor(GameManager.Instance.GetMotivationalColor(value));
            SetTrailColor(GameManager.Instance.GetMotivationalColor(value));
            _bodyController.FootEffect(false);
            if (value > 4)
            {
                _bodyController.FootEffect(true);
            }
        }
    }
}
