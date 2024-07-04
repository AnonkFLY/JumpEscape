using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SerializeField]
public class LevelData
{
    public int maxScore = 500;
    public Color levelColor = Color.white;
}

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get => _instance; }
    [SerializeField] private PlayerManager mainPlayer;
    public PlayerManager GetMainPlayer()
    {
        return mainPlayer;
    }

    [Header("Requirement")]
    [SerializeField] private UIAssets _uiAssets;
    [SerializeField] private string _startOpenUI;
    [SerializeField] private LevelData[] _levelData;
    private ArchiveManager<GameSave> _archiveManager = new ArchiveManager<GameSave>();
    private void Awake()
    {
        SingleInit();
        //Init Other
        Application.targetFrameRate = -1;
        UIManager.Instance.LoadUIResource(_uiAssets);
        if (!string.IsNullOrEmpty(_startOpenUI))
            UIManager.Instance.Open(_startOpenUI);
        Load();
        InputHandler.Instance.onClickEvent += mainPlayer.OnClick;
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
