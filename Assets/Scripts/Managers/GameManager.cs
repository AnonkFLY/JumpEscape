using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get => _instance; }
    [SerializeField] private PlayerManager _mainPlayer;
    public PlayerManager GetMainPlayer()
    {
        return _mainPlayer;
    }
    private SceneManager _sceneManager = new SceneManager();
    public SceneManager GetSceneManager()
    {
        return _sceneManager;
    }
    [Header("LevelConfigs")]
    [SerializeField] private LevelConfig[] _levelConfigs;
    [Header("PlayerTypes")]
    [SerializeField] private GameObject[] _playerTypes;

    [Header("Requirement")]
    [SerializeField] private UIAssets _uiAssets;
    /*[SerializeField]*/
    private string _startOpenUI;

    [SerializeField] private GameObject _sceneObj;
    private ArchiveManager<GameSave> _archiveManager = new ArchiveManager<GameSave>();

    private MotivationalManager _motivaManager = new MotivationalManager();

    private GamePlayerUI _gamePlayerUI;
    private void Awake()
    {
        SingleInit();
        //Init Other
        PreLoadInit();
    }
    private void Update()
    {
        _sceneManager.Update();
        if (Input.GetKeyDown(KeyCode.A))
        {
            AddScore(1000, 0);
        }
        UpdateScore();
    }
    private CameraManager _cameraManager;

    private void PreLoadInit()
    {
        Application.targetFrameRate = -1;
        //UIInit
        UIManager.Instance.LoadUIResource(_uiAssets);
        _gamePlayerUI = UIManager.Instance.PreLoadUI("GamePlayerUI") as GamePlayerUI;
        if (!string.IsNullOrEmpty(_startOpenUI))
            UIManager.Instance.Open(_startOpenUI);
        //Load save data
        Load();

        _cameraManager = Camera.main.GetComponent<CameraManager>();
        SetPlayerObj();
        _sceneManager.Init(GameObject.Instantiate(_sceneObj).transform);
        UpdateSceneManagerAndUI();
    }
    public LevelConfig GetCurrentLevelConfig()
    {
        return _levelConfigs[_archiveManager.archiveObj.level - 1];
    }
    public Color GetCurrentLevelColor()
    {
        return GetCurrentLevelConfig().color;
    }
    public void UpdateSceneManagerAndUI()
    {
        var levelConfig = GetCurrentLevelConfig();
        _sceneManager.InitSceneObject(levelConfig, _mainPlayer);
        _sceneManager.SetLevelTag(_archiveManager.archiveObj.level);
        _gamePlayerUI.SetColor(levelConfig.color);
        _gamePlayerUI.ScoreBarUIView.SetLevel(_archiveManager.archiveObj.level);
        _gamePlayerUI.ScoreTextUIView.SetMaxScore(levelConfig.levelScoreRequire);
        _gamePlayerUI.ScoreTextUIView.OpenMaxScore();

        _motivaManager.scoreRequire = levelConfig.levelScoreRequire;
        ResetScore();
    }
    private void UpdateScore()
    {
        float p = (_mainPlayer.transform.position.y / -GetCurrentLevelConfig().levelLength);
        if (p > 1.0f)
            p = 1.0f;
        _gamePlayerUI.SetScoreUI(_motivaManager.currentScore,
            _archiveManager.archiveObj.currentLevelMaxScore,
            p,
            _archiveManager.archiveObj.currentLevelMaxScorePer,
            _archiveManager.archiveObj.level);
    }
    public void AddScore(int value, int scoreLevel)
    {
        _motivaManager.AddScore(value, scoreLevel);
        _gamePlayerUI.ScoreTextUIView.SetCurrentScore(_motivaManager.currentScore);
        if (_motivaManager.currentScore > _archiveManager.archiveObj.currentLevelMaxScore)
            _archiveManager.archiveObj.currentLevelMaxScore = _motivaManager.currentScore;
    }
    public void ResetScore()
    {
        float p = (_mainPlayer.transform.position.y / -GetCurrentLevelConfig().levelLength);
        if (p > _archiveManager.archiveObj.currentLevelMaxScorePer) _archiveManager.archiveObj.currentLevelMaxScorePer = p;
        _motivaManager.currentScore = 0;
        _gamePlayerUI.ScoreTextUIView.SetCurrentScore(_motivaManager.currentScore);
        _gamePlayerUI.ScoreBarUIView.MaxScoreBar.SetValue(_archiveManager.archiveObj.currentLevelMaxScorePer);
        //UpdateScore();
    }
    public void SetPlayerObj()
    {
        if (_mainPlayer != null)
        {
            InputHandler.Instance.onClickEvent -= _mainPlayer.OnClick;
            Destroy(_mainPlayer.gameObject);
        }
        GameObject playerObject = GameObject.Instantiate(_playerTypes[_archiveManager.archiveObj.selectRole]);
        playerObject.transform.position = Vector3.zero;
        _mainPlayer = playerObject.GetComponent<PlayerManager>();

        _sceneManager.InitPlayer(_mainPlayer);
        InputHandler.Instance.onClickEvent += _mainPlayer.OnClick;
        _cameraManager.SetPlayerManager(_mainPlayer);
        _gamePlayerUI.SetPlayerManager(_mainPlayer);
    }
    //private void InitPlayer()
    //{
    //    InputHandler.Instance.onClickEvent += _mainPlayer.OnClick;
    //}
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
        //Debug.Log("Saved data");
        Save();
    }

}
