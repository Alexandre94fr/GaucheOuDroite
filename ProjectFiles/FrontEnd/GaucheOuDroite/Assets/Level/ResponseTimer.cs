using System;
using System.Collections;
using UnityEngine;


public class ResponseTimer : MonoBehaviour
{
    [Header("----- DEBUG -----")]
    [SerializeField] bool _isDebugModeOn;


    Coroutine _timerCoroutine;

    float _remainingTime;
    bool _isPaused;


    bool IsTimerAlreadyRunning()
    {
        if (_timerCoroutine == null)
            return false;     

        return true;
    }


    public float GetRemainingTime()
    {
        return _remainingTime;
    }

    void SetRemainingTime(float p_newValue)
    {
        _remainingTime = p_newValue;

        EventHandler.OnRemainingResponseTimeChangedEvent?.Invoke(_remainingTime);
    }

    public bool GetIsPaused()
    {
        return _isPaused;
    }


    public void StartTimer(float p_timeOutInSeconds, Action p_onTimeOut)
    {
        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Trying to start the timer with a time out of {p_timeOutInSeconds}s.");


        if (IsTimerAlreadyRunning())
        {
            Debug.LogWarning($"WARNING: [{GetType().Name}] The timer is already running. Returning.");
            return;
        }

        _remainingTime = p_timeOutInSeconds;
        _isPaused = false;

        EventHandler.OnMaximumRemainingResponseTimeChangedEvent?.Invoke(_remainingTime);

        _timerCoroutine = StartCoroutine(TimerCoroutine(p_onTimeOut));


        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Successfully started the timer.");
    }

    public void PauseTimer()
    {
        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Trying to pause the timer.");


        if (!IsTimerAlreadyRunning())
        {
            Debug.LogWarning($"WARNING: [{GetType().Name}] The timer is not running. Returning.");
            return;
        }

        _isPaused = true;


        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Successfully paused the timer.");
    }

    public void ResumeTimer()
    {
        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Trying to resume the timer.");


        if (!IsTimerAlreadyRunning())
        {
            Debug.LogWarning($"WARNING: [{GetType().Name}] The timer is not running. Returning.");
            return;
        }

        _isPaused = false;


        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Successfully resumed the timer.");
    }

    public void StopTimer()
    {
        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Trying to stop the timer.");


        if (!IsTimerAlreadyRunning())
        {
            Debug.LogWarning($"WARNING: [{GetType().Name}] The timer is not running. Returning.");
            return;
        }

        StopCoroutine(_timerCoroutine);

        _timerCoroutine = null;
        SetRemainingTime(0);
        _isPaused = false;


        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Successfully stopped the timer.");
    }


    IEnumerator TimerCoroutine(Action p_onTimeOut)
    {
        while (_remainingTime > 0f)
        {
            if (!_isPaused)
            {
                SetRemainingTime(_remainingTime - Time.deltaTime);
            }

            yield return null;
        }

        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] The Timer timed out. Resetting class values and invoking the callback.");

        _timerCoroutine = null;
        SetRemainingTime(0);
        _isPaused = false;

        p_onTimeOut?.Invoke();
    }
}