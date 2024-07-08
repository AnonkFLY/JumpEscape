using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneEntity : MonoBehaviour
{
    [SerializeField] private string entityName = "BaseEntity";

    /// <summary> 
    /// ��������ʱ����һ�θú���(������ʵ����Ӧ�õ��ø÷���)
    /// </summary>
    public abstract void CreateEntitys(LevelConfig levelConfig, PlayerManager playerManager,SceneManager sceneManager);

    /// <summary>
    /// �ú�����������ʵ���ķ�ʽ,������ʵ����������
    /// </summary>
    public abstract void Init(LevelConfig levelConfig, PlayerManager playerManager, SceneManager sceneManager);
}
