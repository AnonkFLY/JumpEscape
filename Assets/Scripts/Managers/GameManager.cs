using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;



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
    [SerializeField] private LanguageScriptobject _languageData;
    [SerializeField] private UIAssets _uiAssets;
    [Header("ScoreColors")]
    [SerializeField] private Color[] _scoreColor;

    /*[SerializeField]*/
    private string _startOpenUI;

    [SerializeField] private GameObject _sceneObj;
    private ArchiveManager<GameSave> _archiveManager = new ArchiveManager<GameSave>();

    private MotivationalManager _motivaManager = new MotivationalManager();

    private GamePlayerUI _gamePlayerUI;

    public event UnityAction<LanguageType> onLanguageChange;
    public UnityAction<int> onMotivational;
    public bool InitDone = false;
    private float p;
    private void Awake()
    {
        var test = FindObjectOfType<TestUI>();
        test.Init();
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
        if (Input.GetKeyDown(KeyCode.W))
        {
            _mainPlayer.unconqueredState = !_mainPlayer.unconqueredState;
            Debug.Log("unconqueredState:" + _mainPlayer.unconqueredState);
        }
        UpdateScore();
    }
    private CameraManager _cameraManager;

    private void PreLoadInit()
    {
        Application.targetFrameRate = 60;
        //UIInit
        Debug.Log(UIManager.Instance);
        UIManager.Instance.LoadUIResource(_uiAssets);
        _gamePlayerUI = UIManager.Instance.PreLoadUI("GamePlayerUI") as GamePlayerUI;
        if (!string.IsNullOrEmpty(_startOpenUI))
            UIManager.Instance.Open(_startOpenUI);
        //Load save data
        Load();
        ChangeLanguage(_archiveManager.archiveObj.isChinese);

        _cameraManager = Camera.main.GetComponent<CameraManager>();
        SetPlayerObj();
        _sceneManager.Init(GameObject.Instantiate(_sceneObj).transform);
        _sceneManager.onSceneOver += OnSceneOver;
        UpdateSceneManagerAndUI();
        InitDone = true;
    }

    public void ChangeLanguage(bool isChinese)
    {
        _archiveManager.archiveObj.isChinese = (bool)isChinese;

        onLanguageChange?.Invoke(_archiveManager.archiveObj.isChinese ? LanguageType.Chinese : LanguageType.English);
    }

    public LevelConfig GetCurrentLevelConfig()
    {
        return _levelConfigs[_archiveManager.archiveObj.level - 1];
    }
    public GameSave GetGameSave()
    {
        return _archiveManager.archiveObj;
    }
    public Color GetCurrentLevelColor()
    {
        return GetCurrentLevelConfig().color;
    }
    public Color GetMotivationalColor(int index)
    {
        return _scoreColor[index - 1];
    }
    public Color GetCurrentRandomColor()
    {
        if (_archiveManager.archiveObj.GetCurrentRandomColor == Color.white)
            _archiveManager.archiveObj.GetCurrentRandomColor = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f); ;
        return _archiveManager.archiveObj.GetCurrentRandomColor;
    }
    public string GetLanguage(LanguageKey key, params object[] para)
    {

        return _languageData.GetLanguageStr(_archiveManager.archiveObj.isChinese ? LanguageType.Chinese : LanguageType.English, key, para);
    }

    public void UpdateSceneManagerAndUI()
    {
        ResetScore();
        var levelConfig = GetCurrentLevelConfig();
        _sceneManager.InitSceneObject(levelConfig, _mainPlayer);
        _sceneManager.SetLevelTag(_archiveManager.archiveObj.level);
        _gamePlayerUI.SetColor(levelConfig.color);
        _gamePlayerUI.ScoreBarUIView.SetLevel(_archiveManager.archiveObj.level);
        //_gamePlayerUI.ScoreTextUIView.SetMaxScore(levelConfig.levelScoreRequire);
        _gamePlayerUI.ScoreTextUIView.OpenMaxScore();

        //_motivaManager.scoreRequire = levelConfig.levelScoreRequire;
    }
    private void UpdateScore()
    {
        if (_motivaManager.ratingTimer >= 0)
            _motivaManager.ratingTimer -= Time.deltaTime;
        if (_motivaManager.ratingTimer <= 0)
        {
            _motivaManager.currentRating = 1;
            if (lastRating != _motivaManager.currentRating)
            {
                onMotivational?.Invoke(_motivaManager.currentRating);
                lastRating = _motivaManager.currentRating;
            }
        }
        p = (_mainPlayer.transform.position.y / -GetCurrentLevelConfig().levelLength);
        if (p >= 1.0f)
            p = 1.0f;
        _gamePlayerUI.SetScoreUI(_motivaManager.currentScore,
            _archiveManager.archiveObj.currentLevelMaxScore,
            p,
            _archiveManager.archiveObj.currentLevelMaxScorePer,
            _archiveManager.archiveObj.level);
    }
    int lastRating = 0;
    public int AddScore(int value, int scoreLevel)
    {
        _motivaManager.AddScore(value, scoreLevel);
        _gamePlayerUI.ScoreTextUIView.SetCurrentScore(_motivaManager.currentScore);
        if (_motivaManager.currentScore > _archiveManager.archiveObj.currentLevelMaxScore)
            _archiveManager.archiveObj.currentLevelMaxScore = _motivaManager.currentScore;
        if (lastRating != _motivaManager.currentRating)
        {
            onMotivational?.Invoke(_motivaManager.currentRating);
            lastRating = _motivaManager.currentRating;
        }
        return _motivaManager.currentRating;
    }
    public void ResetScore()
    {
        float p = (_mainPlayer.transform.position.y / -GetCurrentLevelConfig().levelLength);
        if (p > _archiveManager.archiveObj.currentLevelMaxScorePer && p < 1.0f) _archiveManager.archiveObj.currentLevelMaxScorePer = p;
        _motivaManager.currentScore = 0;
        onMotivational?.Invoke(1);
        _gamePlayerUI.ScoreTextUIView.SetCurrentScore(_motivaManager.currentScore);
        _gamePlayerUI.ScoreBarUIView.MaxScoreBar.SetValue(_archiveManager.archiveObj.currentLevelMaxScorePer);
    }
    public void SetPlayerObj()
    {
        if (_mainPlayer != null)
        {
            InputHandler.Instance.onClickEvent -= _mainPlayer.OnClick;
            onMotivational -= _mainPlayer.OnMotivational;
            Destroy(_mainPlayer.gameObject);
        }
        GameObject playerObject = GameObject.Instantiate(_playerTypes[_archiveManager.archiveObj.selectRole]);

        playerObject.transform.position = Vector3.zero;

        _mainPlayer = playerObject.GetComponent<PlayerManager>();
        //_mainPlayer.GetBodyController().SetMainColor(GetCurrentLevelColor());
        _mainPlayer.GetBodyController().SetSecondColor(_archiveManager.archiveObj.GetCurrentRandomColor);
        //_mainPlayer.SetTrailColor(GetCurrentLevelColor());
        _mainPlayer.OnMotivational(1);


        _sceneManager.InitPlayer(_mainPlayer);
        InputHandler.Instance.onClickEvent += _mainPlayer.OnClick;
        onMotivational += _mainPlayer.OnMotivational;
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
    public void SetColor()
    {
        _archiveManager.archiveObj.GetCurrentRandomColor = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f); ;

        _mainPlayer.GetBodyController().SetMainColor(GetCurrentLevelColor());
        _mainPlayer.GetBodyController().SetSecondColor(_archiveManager.archiveObj.GetCurrentRandomColor);
        _mainPlayer.SetTrailColor(GetCurrentLevelColor());
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
    public bool NextLevel()
    {
        if (p >= 1.0f)
        {
            ++_archiveManager.archiveObj.level;
            _archiveManager.archiveObj.currentLevelMaxScore = 0;
            _archiveManager.archiveObj.currentLevelMaxScorePer = 0;
            SetColor();
            if (_archiveManager.archiveObj.level > _levelConfigs.Count())
            {
                _archiveManager.archiveObj.level = 1;
                return true;
            }
        }
        return false;
    }

    private void OnSceneOver(SceneManager sceneManager)
    {
        //StartCoroutine();
        _gamePlayerUI.ResetUI(true);
    }
}
