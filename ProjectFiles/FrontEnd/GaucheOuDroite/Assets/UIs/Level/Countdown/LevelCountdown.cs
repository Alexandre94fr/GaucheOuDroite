using System;
using System.Collections;
using TMPro;
using UnityEngine;

using VariableCheckerPackage;


public class LevelCountdown : MonoBehaviour
{
    [Header("----- DEBUG -----:")]
    [SerializeField] bool _isDebugModeOn = false;


    [Header("Internal references:")]
    [SerializeField] GameObject _gameOverUIGameObject;

    [Space]
    [SerializeField] TextMeshProUGUI _counterText;


    [Header("Properties:")]
    [SerializeField] int _numberOfSeconds = 3;


    void Start()
    {
        // -- Class properties verifications -- //

        if (!VariablesChecker.AreVariablesValid(name, null,
            (_gameOverUIGameObject, nameof(_gameOverUIGameObject)),

            (_counterText, nameof(_counterText))
        )) return;

        if (_numberOfSeconds <= 0)
        {
            Debug.LogWarning($"WARNING: [{GetType().Name}] The '{nameof(_numberOfSeconds)}' property is equal or inferior to 0. Returning.");
            return;
        }
    }


    public void StartCountdown(float p_timeInSecondsPerCooldownSecond, Action p_onCountdownFinished)
    {
        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Starting the countdown coroutine and making the countdown UI visible.");

        _gameOverUIGameObject.SetActive(true);

        StartCoroutine(CountdownCoroutine(p_timeInSecondsPerCooldownSecond, p_onCountdownFinished));
    }

    IEnumerator CountdownCoroutine(float p_timeInSecondsPerCooldownSecond, Action p_onCountdownFinished)
    {
        int numberOfSecondsFinished = 0;

        while (numberOfSecondsFinished < _numberOfSeconds)
        {
            _counterText.text = (_numberOfSeconds - numberOfSecondsFinished).ToString();

            yield return new WaitForSeconds(p_timeInSecondsPerCooldownSecond);

            numberOfSecondsFinished++;
        }


        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] The countdown coroutine has ended. Hidding the countdown UI and invoking the callback.");

        _gameOverUIGameObject.SetActive(false);

        p_onCountdownFinished?.Invoke();
    }
}