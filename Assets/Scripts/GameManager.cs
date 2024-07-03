using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get => _instance; }

    [Header("Requirement")]
    [SerializeField] private UIAssets _uiAssets;
    [SerializeField] private string _startOpenUI;
    private ArchiveManager<GameSave> _archiveManager = new ArchiveManager<GameSave>();
    private void Awake()
    {
        SingleInit();
        //Init Other
        UIManager.Instance.LoadUIResource(_uiAssets);
        if (!string.IsNullOrEmpty(_startOpenUI))
            UIManager.Instance.Open(_startOpenUI);
        Load();
    }
    public GameSave GetSave()
    {
        return _instance?._archiveManager?.archiveObj;
    }
    public void Save()
    {
        _instance?._archiveManager?.Saved();
    }
    public void Load()
    {
        _instance?._archiveManager?.Load();
    }
    private void SingleInit()
    {
        if (_instance == null)
            _instance = this;
        if (_instance != this)
            Destroy(gameObject);
    }
    private void OnDestroy()
    {
        Debug.Log("Saved data");
        Save();
    }

}
