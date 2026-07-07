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

        // -- Checking if the given Id is correct by trying to get the Level's properties and LevelProgression -- //

        _gameDataManager = GameDataManager.Instance;
        _userDataManager = UserDataManager.Instance;

        if (!_gameDataManager.TryGetLevel(p_levelId, out _levelProperties))
        {
            Debug.LogWarning($"WARNING: [{GetType().Name}] Failed to get the {nameof(Level)} {p_levelId}. There is no {nameof(Level)} associated with {nameof(Level)} Id: {p_levelId}. Initialization failed. Returning.");
            return;
        }

        if (!_userDataManager.TryGetLevelProgression(p_levelId, out _levelProgression))
        {
            Debug.LogWarning($"WARNING: [{GetType().Name}] Failed to get the {nameof(LevelProgression)} {p_levelId}. There is no {nameof(LevelProgression)} associated with the {nameof(Level)} Id: {p_levelId}. Initialization failed. Returning.");
            return;
        }

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

    public void StartNextLevel()
    {
        StartNewLevel(_currentLevelId + 1);
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
            Debug.Log($"DEBUG: [{GetType().Name}] Received the '{nameof(GameplayLoopManager.OnGameplayLoopWonEvent)}' Event call.");

        int score = ScoreManager.GetScore();

        bool isNextLevelUnlocked = TrySaveNewBestScore(score);

        bool isPreviousBestScoreBeaten = TryUnlockNextLevel();

        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Invoking the {nameof(EventHandler.OnLevelWonEvent)} Event.");

        EventHandler.OnLevelWonEvent?.Invoke(_levelProperties, isNextLevelUnlocked, score, isPreviousBestScoreBeaten);
    }

    void OnGameplayLoopLost()
    {
        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Received the '{nameof(GameplayLoopManager.OnGameplayLoopLostEvent)}' Event call.");

        int score = ScoreManager.GetScore();

        bool isPreviousBestScoreBeaten = TrySaveNewBestScore(score);

        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Invoking the {nameof(EventHandler.OnLevelLostEvent)} Event.");

        // When you lost a Level, you never unlock the next Level.
        EventHandler.OnLevelLostEvent?.Invoke(_levelProperties, false, score, isPreviousBestScoreBeaten);
    }


    bool TryUnlockNextLevel()
    {
        int nextLevelId = _currentLevelId + 1;

        if (!_userDataManager.TryGetLevelProgression(nextLevelId, out LevelProgression nextLevelProgression))
        {
            if (_isDebugModeOn)
                Debug.Log($"DEBUG: [{GetType().Name}] Failed to get the {nameof(LevelProgression)} {nextLevelId}. There is no {nameof(LevelProgression)} associated with the {nameof(Level)} Id: {nextLevelId}. Returning false.");
            
            return false;
        }

        if (nextLevelProgression.IsUnlocked == true)
        {
            if (_isDebugModeOn)
                Debug.Log($"DEBUG: [{GetType().Name}] The {nameof(Level)} {nextLevelId} is already unlocked. Returning false.");

            return false;
        }

        nextLevelProgression.IsUnlocked = true;
        _userDataManager.UpdateLevelProgression(nextLevelId, nextLevelProgression);

        StartCoroutine(_userDataManager.SaveAllUserDataAsync());

        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Successfully sent a request to the server to save that next {nameof(Level)} (Id: {nextLevelId}) is unlocked. Returning true.");

        return true;
    }

    bool TrySaveNewBestScore(int p_score)
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