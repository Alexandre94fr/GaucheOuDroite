using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using VariableCheckerPackage;


public class LevelCountdown : MonoBehaviour
{
    [Header("----- DEBUG -----:")]
    [SerializeField] bool _isDebugModeOn = false;


    [Header("Internal references:")]
    [SerializeField] AudioSource _audioSource;

    [Space]
    [SerializeField] GameObject _gameOverUIGameObject;

    [Space]
    [SerializeField] TextMeshProUGUI _counterText;


    [Header("Properties:")]
    [SerializeField] int _numberOfSeconds = 3;

    [SerializeField] List<AudioClip> _countdownSFXs = new();


    void Start()
    {
        // -- Class properties verifications -- //

        if (!VariablesChecker.AreVariablesValid(name, null,
            (_audioSource, nameof(_audioSource)),

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
            // -- Text -- //

            _counterText.text = (_numberOfSeconds - numberOfSecondsFinished).ToString();

            // -- Countdown SFX -- //

            TryPlayCountdownSFX(numberOfSecondsFinished);

            yield return new WaitForSeconds(p_timeInSecondsPerCooldownSecond);

            numberOfSecondsFinished++;
        }

        // -- Countdown SFX end -- //

        TryPlayCountdownSFX(numberOfSecondsFinished);


        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] The countdown coroutine has ended. Hidding the countdown UI and invoking the callback.");

        _gameOverUIGameObject.SetActive(false);

        p_onCountdownFinished?.Invoke();
    }

    bool TryPlayCountdownSFX(int p_numberOfSecondsFinished)
    {
        AudioClip sfx;

        try
        {
            sfx = _countdownSFXs[p_numberOfSecondsFinished];
        }
        catch (Exception exception)
        {
            Debug.LogWarning($"WARNING: [{GetType().Name}] There is no countdown SFX with the index {p_numberOfSecondsFinished} inside the '{nameof(_countdownSFXs)}' property. No SFX will be played. Returning false.\nException: {exception}");
            return false;
        }

        _audioSource.clip = sfx;
        _audioSource.Play();

        return true;
    }
}