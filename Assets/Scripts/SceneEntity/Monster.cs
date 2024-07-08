using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Monster : SceneEntityPool<Monster>, ICanDestroy
{
    [SerializeField] private float maxSizeScale = 1.3f;
    [SerializeField] private int addScore = 30;

    private WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

    [Header("CreateParamater")]
    [SerializeField] private float createInterval = 0.3f;


    private bool isTrigger = false;
    private TMP_Text _scoreText;
    private Canvas _canves;
    private Vector3 _originPos;
    private Transform _effectPoint;
    private SpriteRenderer _effectSpriteRenderer;
    private Color _originEffectAlpha;

    public override void CreateEntitys(LevelConfig levelConfig, PlayerManager playerManager, SceneManager sceneManager)
    {
        GameManager.Instance.StartCoroutine(StartCreateMonster(levelConfig, playerManager, sceneManager));
    }
    private void OnSceneOver(SceneManager sceneManager)
    {
        DestroyEntity();
        sceneManager.onSceneInit -= OnSceneOver;
    }
    [SerializeField]
    private int[] diffCount = new int[5] { 135, 170, 190, 220, 300 };
    IEnumerator StartCreateMonster(LevelConfig levelConfig, PlayerManager playerManager, SceneManager sceneManager)
    {
        int count = diffCount[(int)levelConfig.diffcult];
        Debug.Log("Create count:" + count);
        for (int i = 0; i < count; i++)
        {
            Create(levelConfig, playerManager, sceneManager);
        }
        yield return waitForEndOfFrame;
    }
    private SpriteRenderer _head;
    private SpriteRenderer _face;
    public static int count = 0;
    public override void Init(LevelConfig levelConfig, PlayerManager playerManager, SceneManager sceneManager)
    {
        if (!_transform)
        {
            _transform = transform;
            _transform.name = _transform.name + count.ToString();
            ++count;
            var sprites = GetComponentsInChildren<SpriteRenderer>();
            _head = sprites[0];
            _face = sprites[1];
            _canves = GetComponentInChildren<Canvas>();
            _canves.worldCamera = Camera.main;
            _scoreText = GetComponentInChildren<TMP_Text>();
            _originPos = _scoreText.rectTransform.localPosition;
            _effectPoint = _transform.Find("EffectPoint").transform;
            _effectSpriteRenderer = _effectPoint.GetComponentInChildren<SpriteRenderer>();
            _originEffectAlpha = _effectSpriteRenderer.color;
        }
        _effectSpriteRenderer.color = new Color(0, 0, 0, 0);
        sceneManager.onSceneInit += OnSceneOver;
        isTrigger = false;
        _head.color = GameManager.Instance.GetCurrentRandomColor();
        Vector3 pos;
        do
        {
            float randomX = Random.Range((int)(-15 / createInterval), (int)(15 / createInterval)) * createInterval;
            float randomY = Random.Range((int)(-15.0f / createInterval), (int)((-levelConfig.levelLength + 5.0f) / createInterval)) * createInterval;

            pos = new Vector3(randomX, randomY, randomY * 0.01f);
            var obj = Physics2D.OverlapCircle((Vector2)pos, 3);
            if (obj)
            {
                //Debug.Log(obj.name + pos + obj.transform.position);
                continue;
            }
            _transform.position = pos;
            break;
        } while (true);


        float scale = Random.Range(1.0f, maxSizeScale);
        _canves.transform.localScale = new Vector2(0.01f / scale, 0.01f / scale);
        _transform.localScale = new Vector2(scale, scale);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTrigger)
            return;
        isTrigger = true;
        int rating = GameManager.Instance.AddScore(addScore, 1);

        //TODO:Ч��
        _scoreText.text = (rating * addScore).ToString();
        _scoreText.color = GameManager.Instance.GetRattingColor(rating);

        Vector3 scale = _transform.localScale;
        _transform.DOScale(scale * 1.5f, 0.15f).OnComplete(() =>
        {
            _transform.DOScale(scale, 0.1f);
        });
        _scoreText.rectTransform.DOLocalMoveY(_originPos.y + 30, 0.7f).OnComplete(() =>
        {
            _scoreText.rectTransform.DOLocalMoveY(_originPos.y + 30, 0.4f).OnComplete(() =>
            {
                _scoreText.text = "";
                _scoreText.rectTransform.localPosition = _originPos;
            });
        });
        _effectSpriteRenderer.color = _originEffectAlpha;
        _effectPoint.DOScale(Vector3.one * 1.6f, 0.65f).OnComplete(() =>
        {
            _effectSpriteRenderer.color = new Color(0, 0, 0, 0);
            _effectPoint.localScale = Vector3.one;
        });
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("Kill player");
        //TODO:��ɱ���
        var player = collision.transform.GetComponent<PlayerManager>();
        if (player != null)
        {
            player.PlayerHurt(DamageType.EntityDamage, gameObject);
        }
    }

    public void Destroy()
    {
        //TODO:���ƻ�
    }
}
