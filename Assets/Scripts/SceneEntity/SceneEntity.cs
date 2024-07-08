using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneEntity : MonoBehaviour
{
    [SerializeField] private string entityName = "BaseEntity";

    /// <summary> 
    /// 场景生成时调用一次该函数(场景内实例不应该调用该方法)
    /// </summary>
    public abstract void CreateEntitys(LevelConfig levelConfig, PlayerManager playerManager,SceneManager sceneManager);

    /// <summary>
    /// 该函数决定复用实例的方式,和生成实体的随机属性
    /// </summary>
    public abstract void Init(LevelConfig levelConfig, PlayerManager playerManager, SceneManager sceneManager);
}
