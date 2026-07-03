using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

using InstantiatorPackage;
using VariableCheckerPackage;

using FrontEnd.Data.Game;
using FrontEnd.Data.User;


public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;


    [Header("----- DEBUG -----")]
    [SerializeField] bool _isDebugModeOn;


    [Header("Internal references:")]
    public ScoreManager ScoreManager;
    public ResponseSequenceManager ResponseSequenceManager;
    public ResponseTimer ResponseTimer;
    public GameplayLoopManager GameplayLoopManager;

    [Space]
    [SerializeField] SceneChanger _sceneChanger;


    [Header("Properties:")]
    [SerializeField] string _levelSceneName = "Level";


    GameDataManager _gameDataManager;
    UserDataManager _userDataManager;

    Level _levelProperties;
    LevelProgression _levelProgression;


    int _currentLevelId = -1;


    void Awake()
    {
        Instance = Instantiator.GetInstance(this, Instantiator.InstanceConflictResolutions.DestroyingDuplicateObject);

        DontDestroyOnLoad(this);
    }

    void Start()
    {
        // -- Class properties verifications -- //

        if (!VariablesChecker.AreVariablesValid(name, null,
            (ScoreManager, nameof(ScoreManager)),
            (ResponseSequenceManager, nameof(ResponseSequenceManager)),
            (ResponseTimer, nameof(ResponseTimer)),
            (GameplayLoopManager, nameof(GameplayLoopManager)),

            (_sceneChanger, nameof(_sceneChanger))
        )) return;

        if (string.IsNullOrEmpty(_levelSceneName))
        {
            Debug.LogError(
                $"{VariablesChecker.GetCheckErrorMessagePrefix(name, nameof(_levelSceneName))} is null or empty, " +
                $"please set it through the Unity inspector. Returning."
            );
            return;
        }

        // -- Handling events -- //

        GameplayLoopManager.OnGameplayLoopWonEvent += OnGameplayLoopWon;
        GameplayLoopManager.OnGameplayLoopLostEvent += OnGameplayLoopLost;
    }

    void OnDestroy()
    {
        GameplayLoopManager.OnGameplayLoopWonEvent -= OnGameplayLoopWon;
        GameplayLoopManager.OnGameplayLoopLostEvent -= OnGameplayLoopLost;
    }


    public int GetCurrentLevelId()
    {
        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Returning the content of the '{nameof(_currentLevelId)}' property: {_currentLevelId}.");


        return _currentLevelId;
    }


    public void StartNewLevel(int p_levelId)
    {
        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Starting a new Level (Id: {p_levelId}).");


        _currentLevelId = p_levelId;

        // -- Loading the Level Scene -- //

        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Starting to load the '{_levelSceneName}' Scene.");

        // To know when the Scene has finished loading.
        // We will unsubscribe from the event when the OnSceneLoaded method is called.
        _sceneChanger.OnSceneLoadedAfterStartCallEvent += OnSceneLoaded;

        _sceneChanger.LoadAsync(_levelSceneName);

        // Check out the OnSceneLoaded method to know what happens when the Level Scene has loaded.
    }

    public void RestartLevel()
    {
        StartNewLevel(_currentLevelId);
    }

    void OnSceneLoaded(Scene p_scene, LoadSceneMode p_loadSceneMode)
    {
        if (p_scene.name != _levelSceneName)
        {
            Debug.LogWarning(
                $"WARNING: [{GetType().Name}] While waiting for the '{_levelSceneName}' Scene to load, another Scene '{p_scene.name}' as loaded. " +
                $"No other Scene should be loading while loading the '{_levelSceneName}' Scene. Returning."
            );
            return;
        }

        _sceneChanger.OnSceneLoadedAfterStartCallEvent -= OnSceneLoaded;

        InitializeLevelManagers();
    }

    void InitializeLevelManagers()
    {
        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Starting to initialize the different Level's managers.");

        _gameDataManager = GameDataManager.Instance;
        _userDataManager = UserDataManager.Instance;


        _levelProperties = _gameDataManager.GetLevel(_currentLevelId);
        _levelProgression = _userDataManager.GetLevelProgression(_currentLevelId);

        ScoreManager.Initialize(_levelProperties, _levelProgression);
        ResponseSequenceManager.Initialize(_levelProperties);
        GameplayLoopManager.Initialize(_levelProperties, ResponseSequenceManager, ResponseTimer, ScoreManager);


        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Successfully initialized the different Level's managers.");

        StartCoroutine(StartLevel());
    }


    IEnumerator StartLevel()
    {
        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Starting to start the Level (Id: {_currentLevelId}).");


        // -- Starting a count-down -- //

        // TODO: Make a count-down, afterward tell the ResponseSequenceManager to start

        // -- Starting the gameplay loop -- //

        GameplayLoopManager.StartGameplayLoop();


        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Successfully started the Level (Id: {_currentLevelId}).");

        yield return null; // TODO: Delete after calling the count-down creation
    }


    void OnGameplayLoopWon()
    {
        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Received the '{nameof(GameplayLoopManager.OnGameplayLoopWonEvent)}' Event call. Invoking the {nameof(EventHandler.OnLevelWonEvent)} Event.");

        int score = ScoreManager.GetScore();

        TrySavingNewBestScore(score);

        // TODO: Make the game unlock the next Level if it exist

        EventHandler.OnLevelWonEvent?.Invoke(_levelProperties, score);
    }

    void OnGameplayLoopLost()
    {
        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Received the '{nameof(GameplayLoopManager.OnGameplayLoopLostEvent)}' Event call. Invoking the {nameof(EventHandler.OnLevelLostEvent)} Event.");

        int score = ScoreManager.GetScore();

        TrySavingNewBestScore(score);

        EventHandler.OnLevelLostEvent?.Invoke(_levelProperties, score);
    }


    bool TrySavingNewBestScore(int p_score)
    {
        if (!ScoreManager.HasNewBestScore())
        {
            if (_isDebugModeOn)
                Debug.Log($"DEBUG: [{GetType().Name}] Tried to save a new best score, but the score of the player is inferior or equal to the current best score. Returning false.");

            return false;
        }

        _levelProgression.BestScore = p_score;
        _userDataManager.UpdateLevelProgression(_currentLevelId, _levelProgression);

        StartCoroutine(_userDataManager.SaveAllUserDataAsync());

        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Successfully sent a request to the server to save a new best score. Returning true.");

        return true;
    }
}