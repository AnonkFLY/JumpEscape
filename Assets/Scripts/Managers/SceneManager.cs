using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public class SceneManager
{
    private bool _levelOver = false;

    private Transform _transform;
    private Transform _endTransform;
    private TMP_Text _endText;
    private TMP_Text _levelTagText;
    private float _playerColliderSize;
    private Vector3 _playerColliderOffset;
    private PlayerManager _playerManager;
    private Transform _playerTransform;
    private Camera _camera;
    private float levelStartTimer;
    private float addScoreTimer;
    private int addScoreValue;
    private LevelConfig _currentLevelConfig;
    private int level = 0;

    public event UnityAction<SceneManager> onSceneOver;
    public event UnityAction<SceneManager> onSceneInit;
    public Transform Transform { get => _transform; }

    public void Init(Transform transform)
    {
        _transform = transform;
        _camera = Camera.main;
        _levelTagText = _transform.Find("LevelTag").GetComponentInChildren<TMP_Text>();
        _endTransform = _transform.Find("End");
        _endText = _endTransform.GetComponentInChildren<TMP_Text>();

    }
    public void InitPlayer(PlayerManager playerManager)
    {
        _playerManager = playerManager;
        _playerTransform = _playerManager.GetComponent<Transform>();
        _playerColliderSize = _playerManager.GetComponent<CircleCollider2D>().radius;
        _playerColliderOffset = _playerManager.GetComponent<CircleCollider2D>().offset;
    }
    public void Update()
    {
        if (_playerManager.IsAlive() && _playerManager.IsActive())
        {
            if (!_playerManager.winState)
            {
                levelStartTimer += Time.deltaTime;
                if (levelStartTimer >= addScoreTimer)
                {
                    levelStartTimer -= addScoreTimer;
                    GameManager.Instance.AddScore(addScoreValue, 0);
                }
                Vector3 left = _playerTransform.position + _playerColliderOffset;
                Vector3 right = left;
                left.x -= _playerColliderSize;
                right.x += _playerColliderSize;
                float xLeftPos = _camera.WorldToScreenPoint(left).x;
                float xRightPos = _camera.WorldToScreenPoint(right).x;
                if (xLeftPos < 0 || xRightPos > Screen.width)
                    _playerManager.PlayerHurt(DamageType.SceneDamage);
            }

            if (!_levelOver && _playerTransform.position.y <= -_currentLevelConfig.levelLength)
            {
                _levelOver = true;
                //TODO:粒子特效
                _endText.text = GameManager.Instance.GetLanguage(LanguageKey.LevelComplate, this.level);
                GameManager.Instance.StartCoroutine(WinCoutine());
            }
        }
    }
    private IEnumerator WinCoutine()
    {
        Debug.Log("过关");
        levelStartTimer = 0;
        Camera.main.GetComponent<CameraManager>().UnLock();
        _playerManager.winState = true;
        for (int i = 0; i < 10; ++i)
        {
            _playerManager.Turning();
            yield return new WaitForSeconds(0.5f);
        }
        onSceneOver?.Invoke(this);
        _endText.text = "";

        yield return new WaitForSeconds(2.5f);
        _levelOver = false;
    }
    public void SetLevelTag(int level)
    {
        _levelTagText.text = level.ToString();
        this.level = level;
        var levelConfig = GameManager.Instance.GetCurrentLevelConfig();
        addScoreTimer = levelConfig.addTimer;
        addScoreValue = levelConfig.addScore;
        levelStartTimer = 0;
    }
    public void InitSceneObject(LevelConfig levelConfig, PlayerManager playerManager)
    {
        onSceneInit?.Invoke(this);
        _endText.text = "";
        _currentLevelConfig = levelConfig;
        var tempPos = _endTransform.position;
        tempPos.y = -levelConfig.levelLength;
        _endTransform.position = tempPos;
        foreach (var item in levelConfig.scenesEntitys)
        {
            item.GetComponent<SceneEntity>().CreateEntitys(levelConfig, playerManager, this);
        }
    }


}
