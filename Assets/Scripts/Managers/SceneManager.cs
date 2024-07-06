using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class SceneManager
{
    private Transform _transform;
    private Transform _endTransform;
    private TMP_Text _levelTagText;
    private float _playerColliderSize;
    private Vector3 _playerColliderOffset;
    private PlayerManager _playerManager;
    private Transform _playerTransform;
    private Camera _camera;
    private float levelStartTimer;
    private float addScoreTimer;
    private int addScoreValue;

    public Transform Transform { get => _transform; }

    public void Init(Transform transform)
    {
        _transform = transform;
        _camera = Camera.main;
        _levelTagText = _transform.Find("LevelTag").GetComponentInChildren<TMP_Text>();
        _endTransform = _transform.Find("End");

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
    }
    public void SetLevelTag(int level)
    {
        _levelTagText.text = level.ToString();
        var levelConfig = GameManager.Instance.GetCurrentLevelConfig();
        addScoreTimer = levelConfig.addTimer;
        addScoreValue = levelConfig.addScore;
        levelStartTimer = 0;
    }
    public void InitSceneObject(LevelConfig levelConfig, PlayerManager playerManager)
    {
        var tempPos = _endTransform.position;
        tempPos.y = -levelConfig.levelLength;
        _endTransform.position = tempPos;
        foreach (var item in levelConfig.scenesEntitys)
        {
            item.GetComponent<SceneEntity>().CreateEntitys(levelConfig, playerManager);
        }
    }


}
