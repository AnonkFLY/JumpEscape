using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : SceneEntityPool<Stone>
{
    [SerializeField] private Vector2 minSpeed;
    [SerializeField] private Vector2 maxSpeed;
    [SerializeField] private float rSpeed = 180;
    [SerializeField] private int createScalue = 2;
    private Vector2 _speed;

    private float _triggerAngle;
    private Transform _bodyTransform;
    private int _direction = 1;
    private bool _isActive = false;
    private PlayerManager _playerManager;
    public override void CreateEntitys(LevelConfig levelConfig, PlayerManager playerManager, SceneManager sceneManager)
    {
        for (int i = 0; i < levelConfig.diffcult * createScalue; i++)
        {
            Create(levelConfig, playerManager, sceneManager);
        }
    }
    private void Update()
    {
        if (!_isActive && Vector2.Angle((_playerManager.transform.position - _transform.position).normalized, _speed.normalized) < _triggerAngle)
        {
            _isActive = true;
        }
        if (_isActive)
        {
            _bodyTransform.Rotate(Vector3.back * rSpeed * Time.deltaTime);
            _transform.Translate(_speed * Time.deltaTime, Space.World);
        }
    }

    public override void Init(LevelConfig levelConfig, PlayerManager playerManager, SceneManager sceneManager)
    {
        if (!_transform)
        {
            _transform = transform;
            _bodyTransform = _transform.Find("Body");
        }
        _playerManager = playerManager;
        _isActive = false;

        _direction = Random.Range(0, 2) == 0 ? -1 : 1;
        _triggerAngle = Random.Range(30.0f, 45.0f);
        if (_direction == -1)
        {
            _transform.rotation = Quaternion.identity;
            _transform.Rotate(Vector3.up * 180);
        }
        else
        {
            _transform.rotation = Quaternion.identity;
        }
        _speed = new Vector2(Random.Range(minSpeed.x, maxSpeed.x), Random.Range(-minSpeed.y, -maxSpeed.y));
        _speed.x *= _direction;

        _transform.position = new Vector2(_direction * -15f, Random.Range(-45.0f, -levelConfig.levelLength + 35.0f));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.transform.GetComponent<PlayerManager>();
        player?.PlayerHurt(DamageType.EntityDamage, gameObject);
    }
}
