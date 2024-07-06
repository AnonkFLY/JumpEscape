using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneEntityPool<T> : SceneEntity where T : SceneEntityPool<T>
{
    protected List<T> _cacheObjects = new List<T>();
    protected Transform _transform;

    public virtual void Create(LevelConfig levelConfig, PlayerManager playerManager)
    {
        T obj = GetObject();
        if (obj == null)
        {
            obj = GameObject.Instantiate(gameObject, GameManager.Instance.GetSceneManager().Transform).GetComponent<T>();
        }
        obj.Init(levelConfig, playerManager);
    }
    protected T GetObject()
    {
        if (_cacheObjects.Count == 0)
        {
            return null;
        }
        T obj = _cacheObjects[_cacheObjects.Count - 1];
        _cacheObjects.RemoveAt(_cacheObjects.Count - 1);
        return obj;
    }
    protected virtual void DestroyEntity()
    {
        gameObject.SetActive(false);
        _cacheObjects.Add((T)this);
    }
}