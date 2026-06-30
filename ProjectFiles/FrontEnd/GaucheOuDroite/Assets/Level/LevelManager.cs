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

    [Space]
    [SerializeField] SceneChanger _sceneChanger;


    [Header("Properties:")]
    [SerializeField] string _levelSceneName = "Level";


    public int _currentLevelId = -1;


    void Awake()
    {
        Instance = Instantiator.GetInstance(this, Instantiator.InstanceConflictResolutions.DestroyingDuplicateObject);

        DontDestroyOnLoad(this);
    }

    void Start()
    {
        // -- Class properties verifications -- //

        if (!VariablesChecker.AreVariablesValid(name, null,
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
        _sceneChanger.OnSceneLoadedEvent += OnSceneLoaded;

        _sceneChanger.LoadAsync(_levelSceneName);

        // Check out the OnSceneLoaded method to know what happens when the Level Scene has loaded.
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

        _sceneChanger.OnSceneLoadedEvent -= OnSceneLoaded;

        InitializeLevelManagers();
    }

    void InitializeLevelManagers()
    {
        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Starting to initialize the different Level's managers.");


        Level levelProperties = GameDataManager.Instance.GetLevel(_currentLevelId);
        LevelProgression levelProgression = UserDataManager.Instance.GetLevelProgression(_currentLevelId);
        
        ScoreManager.Initialize(levelProperties, levelProgression);
        ResponseSequenceManager.Initialize(levelProperties);


        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Successfully initialized the different Level's managers.");

        StartCurrentLevel();
    }


    public void StartCurrentLevel()
    {
        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Starting to start the current Level (Id: {_currentLevelId}).");


        // TODO: Make a count-down, afterward tell the ResponseSequenceManager to start


        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Successfully started the current Level (Id: {_currentLevelId}).");
    }

    public void EndCurrentLevel()
    {
        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Starting to end the current Level (Id: {_currentLevelId}).");


        // TODO: Tells the ScoreManager and ResponseSequenceManager to stop if they are not already stopped
        
        // TODO: Show the Game over UI (with updated data ofc)


        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Successfully ended the current Level (Id: {_currentLevelId}).");
    }
}