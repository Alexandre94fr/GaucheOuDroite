using UnityEngine;

using InstantiatorPackage;
using System;


public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;

    /// <summary>
    /// Parameters' description:
    /// 
    /// <list type="number">
    /// <item><description> bool: Is the game paused. </description></item>
    /// </list>
    /// </summary>
    public static Action<bool> OnPauseEvent;


    [Header("----- DEBUG -----")]
    [SerializeField] bool _isDebugModeOn;


    bool _isGamePaused = false;


    void Awake()
    {
        Instance = Instantiator.GetInstance(this, Instantiator.InstanceConflictResolutions.WarningAndDestroyingDuplicateObject);
    }

    void Start()
    {
        // -- Handling events -- //

        EventHandler.OnPauseInputEvent += OnPauseInput;
    }

    void OnDestroy()
    {
        EventHandler.OnPauseInputEvent -= OnPauseInput;
    }


    public bool GetIsGamePaused()
    {
        return _isGamePaused;
    }


    void OnPauseInput()
    {
        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] The '{nameof(EventHandler.OnPauseInputEvent)}' Event has been invoked, starting to handle the pause.");


        _isGamePaused = !_isGamePaused;

        if (_isGamePaused)
        {
            if (_isDebugModeOn)
                Debug.Log($"DEBUG: [{GetType().Name}] Stopping the time.");

            Time.timeScale = 0.0f;
        }
        else
        {
            if (_isDebugModeOn)
                Debug.Log($"DEBUG: [{GetType().Name}] Resuming the time.");

            Time.timeScale = 1.0f;
        }

        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Invoking the '{nameof(OnPauseEvent)}' Event.");

        OnPauseEvent?.Invoke(_isGamePaused);
    }
}