using System;
using System.Collections;
using UnityEngine;

using VariableCheckerPackage;

using FrontEnd.Data.Game;


public class GameplayLoopManager : MonoBehaviour
{
    // These events should only be used by the GameplayLoopManager and LevelManager.
    // They allow the GameplayLoopManager to tell the LevelManager that the player won or lost.
    public event Action OnGameplayLoopWonEvent;
    public event Action OnGameplayLoopLostEvent;


    [Header("----- DEBUG -----")]
    [SerializeField] bool _isDebugModeOn;

    [Header("Internal references:")]
    [SerializeField] AudioSource _audioSource;

    [Header("Properties:")]
    [SerializeField] AudioClip _correctAwnserSFX;

    [Space]
    [SerializeField] AudioClip _levelWonSFX;
    [SerializeField] AudioClip _levelLostSFX;



    Level _levelProperties;

    ResponseSequenceManager _responseSequenceManager;
    ResponseTimer _responseTimer;
    ScoreManager _scoreManager;


    Coroutine _gameplayLoopCoroutine;

    bool _shouldGameplayLoopStop = false;
    bool _isGamePaused = false;

    bool _hasReceivedPlayerResponse = false;
    DirectionProperties.Direction _playerDirectionResponse = default;


    void Start()
    {
        // -- Class properties verifications -- //

        if (!VariablesChecker.AreVariablesValid(name, null,
            (_audioSource, nameof(_audioSource)),

            (_correctAwnserSFX, nameof(_correctAwnserSFX)),

            (_levelWonSFX, nameof(_levelWonSFX)),
            (_levelLostSFX, nameof(_levelLostSFX))
        )) return;

        // -- Handling events -- //

        EventHandler.OnDirectionChoiceInputEvent += OnDirectionChoiceInput;
        PauseManager.OnPauseEvent += OnPause;
    }

    void OnDestroy()
    {
        EventHandler.OnDirectionChoiceInputEvent -= OnDirectionChoiceInput;
        PauseManager.OnPauseEvent -= OnPause;
    }


    public void Initialize(
        Level p_levelProperties,
        ResponseSequenceManager p_responseSequenceManager, ResponseTimer p_responseTimer, ScoreManager p_scoreManager
    )
    {
        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Starting to initialize the {GetType().Name} Class for Level '{p_levelProperties.Name}'.");

        _levelProperties = p_levelProperties;

        _responseSequenceManager = p_responseSequenceManager;
        _responseTimer = p_responseTimer;
        _scoreManager = p_scoreManager;

        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Successfully initialized the {GetType().Name} Class for Level '{p_levelProperties.Name}'.");
    }

    public bool IsGameplayLoopAlreadyRunning()
    {
        if (_gameplayLoopCoroutine == null)
            return false;

        return true;
    }

    /// <summary>
    /// Should only be called by the <see cref="LevelManager"/>.
    /// </summary>
    public void StartGameplayLoop()
    {
        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Trying to start the gameplay loop.");


        if (IsGameplayLoopAlreadyRunning())
        {
            Debug.LogWarning($"WARNING: [{GetType().Name}] The gameplay loop is already running. Returning.");
            return;
        }

        _gameplayLoopCoroutine = StartCoroutine(GameplayLoop());


        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Successfully started the gameplay loop.");
    }


    float GetMaximalResponseTimeInSeconds(int p_correctResponsesNumber)
    {
        if (_levelProperties.LevelResponseTimeSteps.Count <= 0)
        {
            Debug.LogWarning($"WARNING: [{GetType().Name}] The '{nameof(_levelProperties)}.LevelResponseTimeSteps' property contains a list with 0 or less value. Returning.");
            return 0;
        }

        // Note:
        // The LevelResponseTimeStep.MinimumCorrectResponses are already sorted from the smallest number to the largest.

        float maximalResponseTimeInSeconds = _levelProperties.LevelResponseTimeSteps[0].MaximumResponseTimeInSeconds;

        foreach (LevelResponseTimeStep levelResponseTimeStep in _levelProperties.LevelResponseTimeSteps)
        {
            if (p_correctResponsesNumber >= levelResponseTimeStep.MinimumCorrectResponses)
            {
                maximalResponseTimeInSeconds = levelResponseTimeStep.MaximumResponseTimeInSeconds;
            }
        }
        
        return maximalResponseTimeInSeconds;
    }


    IEnumerator GameplayLoop()
    {
        _shouldGameplayLoopStop = false;

        int correctResponsesNumber = 0;


        while (_shouldGameplayLoopStop == false)
        {
            bool hasTimerTimedOut = false;

            // -- Getting the next correct direction -- //

            if (_isDebugModeOn)
                Debug.Log($"DEBUG: [{GetType().Name}] Trying to get the next correct direction.");

            if (!_responseSequenceManager.TryAdvanceToNextDirection(out DirectionProperties.Direction correctDirection))
            {
                Debug.LogError($"ERROR: [{GetType().Name}] Failed to advance and get the next direction. Making the player lose to avoid more bugs, stopping the gameplay loop.");

                StopGameplayLoop();
                Lose();

                yield break;
            }

            // -- Getting the maximal response time -- //

            if (_isDebugModeOn)
                Debug.Log($"DEBUG: [{GetType().Name}] Getting the maximal response time.");

            float maximalResponseTimeInSeconds = GetMaximalResponseTimeInSeconds(correctResponsesNumber);

            // -- Starting the response timer -- //

            if (_isDebugModeOn)
                Debug.Log($"DEBUG: [{GetType().Name}] Starting the response timer.");

            _responseTimer.StartTimer(
                maximalResponseTimeInSeconds,
                () =>
                {
                    if (_isDebugModeOn)
                        Debug.Log($"DEBUG: [{GetType().Name}] The player failed to respond a direction fast enough. Making the player lose, stopping the gameplay loop.");

                    hasTimerTimedOut = true;
                }
            );

            // -- Yielding the gameplay loop until some conditions are reached -- //

            if (_isDebugModeOn)
                Debug.Log($"DEBUG: [{GetType().Name}] Yielding the gameplay loop until some conditions are reached.");

            _hasReceivedPlayerResponse = false;
            _playerDirectionResponse = default;

            yield return new WaitUntil(
                () =>
                    !_isGamePaused &&
                    (_hasReceivedPlayerResponse || _shouldGameplayLoopStop || hasTimerTimedOut)
            );

            // -- Stopping the gameplay loop if asked -- //

            if (_shouldGameplayLoopStop)
            {
                if (_isDebugModeOn)
                    Debug.Log($"DEBUG: [{GetType().Name}] The '{_shouldGameplayLoopStop}' property has been set to true, stopping the gameplay loop coroutine.");

                yield break;
            }

            if (hasTimerTimedOut)
            {
                if (_isDebugModeOn)
                    Debug.Log($"DEBUG: [{GetType().Name}] The '{hasTimerTimedOut}' variable has been set to true, the timer has timed out. Making the player lose, stopping the gameplay loop.");

                StopGameplayLoop(false);
                Lose();

                yield break;
            }

            // -- Handling incorrect player response -- //

            if (_playerDirectionResponse != correctDirection)
            {
                if (_isDebugModeOn)
                    Debug.Log($"DEBUG: [{GetType().Name}] The player failed to respond the correct direction (Player response: {_playerDirectionResponse}, Correct response: {correctDirection}). Making the player lose, stopping the gameplay loop.");

                StopGameplayLoop();
                Lose();

                yield break;
            }

            // -- Increasing the player correct number of response -- //

            correctResponsesNumber++;

            // -- Computing the score -- //

            if (_isDebugModeOn)
                Debug.Log($"DEBUG: [{GetType().Name}] Computing and adding the computed score based on the remaining time and maximum response time.");

            // Will be used when the score will be computed using the remaining time
            float remainingTime = _responseTimer.GetRemainingTime();

            // Adding a score that is based on the remaining response time
            _scoreManager.AddScoreBasedOnRemainingResponseTime(remainingTime, maximalResponseTimeInSeconds);

            // -- Stopping the timer -- //

            if (_isDebugModeOn)
                Debug.Log($"DEBUG: [{GetType().Name}] Stopping the timer.");

            _responseTimer.StopTimer();

            // -- Handling correct player response -- //

            PlaySFX(_correctAwnserSFX);

            if (!_levelProperties.IsInfinite && correctResponsesNumber >= _responseSequenceManager.GetLevelResponseNumber())
            {
                if (_isDebugModeOn)
                    Debug.Log($"DEBUG: [{GetType().Name}] The player have successfully responded to all direction correctly. Making the player win, stopping the gameplay loop.");

                StopGameplayLoop(false);
                Win();

                yield break;
            }
        }

        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] The gameplay loop has ended.");
    }

    public void StopGameplayLoop(bool p_shouldTryStoppingTimer = true)
    {
        if (!IsGameplayLoopAlreadyRunning())
        {
            Debug.LogWarning($"WARNING: [{GetType().Name}] The gameplay loop is already stopped, '{nameof(_gameplayLoopCoroutine)}' is already null. Returning.");
            return;
        }

        if (p_shouldTryStoppingTimer)
            _responseTimer.StopTimer();

        // The coroutine will stop herself, and reset the other properties back to normal
        _shouldGameplayLoopStop = true;

        _gameplayLoopCoroutine = null;
    }


    void Win()
    {
        PlaySFX(_levelWonSFX);

        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Invoking the '{nameof(OnGameplayLoopWonEvent)}' Event.");

        OnGameplayLoopWonEvent?.Invoke();
    }

    void Lose()
    {
        PlaySFX(_levelLostSFX);

        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Invoking the '{nameof(OnGameplayLoopLostEvent)}' Event.");

        OnGameplayLoopLostEvent?.Invoke();
    }


    void OnDirectionChoiceInput(DirectionProperties.Direction p_direction)
    {
        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] The '{nameof(EventHandler.OnDirectionChoiceInputEvent)}' Event has been fired. Changing the '{nameof(_hasReceivedPlayerResponse)}' property to true and changing the '{nameof(_playerDirectionResponse)}' property to {p_direction}.");

        if (_hasReceivedPlayerResponse)
        {
            if (_isDebugModeOn)
                Debug.Log($"DEBUG: [{GetType().Name}] The player is trying to give a new response even though he already gave one. Returning.");

            return;
        }

        _hasReceivedPlayerResponse = true;
        _playerDirectionResponse = p_direction;
    }

    void OnPause(bool p_isGamePaused)
    {
        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] The '{nameof(PauseManager.OnPauseEvent)}' Event has been fired. Changing the '{nameof(_isGamePaused)}' property from: {_isGamePaused}, to: {p_isGamePaused}.");

        _isGamePaused = p_isGamePaused;
    }

    void PlaySFX(AudioClip p_sfx)
    {
        if (p_sfx == null)
        {
            Debug.LogWarning($"WARNING: [{GetType().Name}] The given SFX is null. No SFX will be played. Returning.");
            return;
        }

        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Playing the '{p_sfx.name}' SFX.");

        _audioSource.clip = p_sfx;
        _audioSource.Play();
    }
}