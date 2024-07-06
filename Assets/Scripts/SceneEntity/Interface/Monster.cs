using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : SceneEntityPool<Monster>, ICanDestroy
{
    [SerializeField] private float maxSizeScale = 1.3f;
    private WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

    [Header("CreateParamater")]
    [SerializeField] private float createInterval = 0.3f;
    HashSet<Vector3> list = new HashSet<Vector3>();
    public bool isTrigger = false;
    public override void CreateEntitys(LevelConfig levelConfig, PlayerManager playerManager)
    {
        list.Clear();
        GameManager.Instance.StartCoroutine(StartCreateMonster(levelConfig, playerManager));
    }
    [SerializeField]
    private int[] diffCount = new int[5] { 135, 170, 190, 220, 300 };
    IEnumerator StartCreateMonster(LevelConfig levelConfig, PlayerManager playerManager)
    {
        int count = diffCount[(int)levelConfig.diffcult];
        Debug.Log("Create count:" + count);
        for (int i = 0; i < count; i++)
        {
            Create(levelConfig, playerManager);
        }
        yield return waitForEndOfFrame;
    }
    private SpriteRenderer _head;
    private SpriteRenderer _face;
    public override void Init(LevelConfig levelConfig, PlayerManager playerManager)
    {
        if (!_transform)
            _transform = transform;
        if (!_head || !_face)
        {
            var sprites = GetComponentsInChildren<SpriteRenderer>();
            _head = sprites[0];
            _face = sprites[1];
        }


        isTrigger = false;

        Vector3 pos;
        do
        {
            float randomX = Random.Range((int)(-15 / createInterval), (int)(15 / createInterval)) * createInterval;
            float randomY = Random.Range((int)(-10.0f / createInterval), (int)((-levelConfig.levelLength + 5.0f) / createInterval)) * createInterval;

            _head.sortingOrder = (int)(-randomY * 30) ;
            _face.sortingOrder = _head.sortingOrder + 1;
            pos = new Vector3(randomX, randomY, randomY * 0.0001f);
            if (!list.Contains(pos))
            {
                _transform.position = pos;
                list.Add(pos);
                break;
            }
        } while (true);


        float scale = Random.Range(1.0f, maxSizeScale);
        _transform.localScale = new Vector2(scale, scale);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTrigger)
            return;
        isTrigger = true;
        print(collision.name);
        //TODO:效果
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("Kill player");
        //TODO:击杀玩家
        var player = collision.transform.GetComponent<PlayerManager>();
        if (player != null)
        {
            player.PlayerHurt(DamageType.EntityDamage, gameObject);
        }
    }

    public void Destroy()
    {
        //TODO:被破坏
    }
}
