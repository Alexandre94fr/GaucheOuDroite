using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using VariableCheckerPackage;

using Shared.Constants;
using Shared.Enums;


public class MaximumResponseTimeModificationNotifier : MonoBehaviour
{
    [Header("----- DEBUG -----")]
    [SerializeField] bool _isDebugModeOn;


    [Header("Internal references:")]
    [SerializeField] GameObject _uiGameObject;
    [SerializeField] Image _backgroundImage;

    [Space]
    [SerializeField] AudioSource _audioSource;

    [Header("Properties:")]
    [SerializeField] bool _isBlinking = false;

    [Space]
    [SerializeField] float _blinkTimeInSeconds = 0.15f;
    [SerializeField] float _backgroundImageTransparency = 0.10f;

    [Space]
    [SerializeField] AudioClip _timeAcceleratingSFX;


    float _maximumRemainingTime = -1;


    void Start()
    {
        // -- Class properties verifications -- //

        if (!VariablesChecker.AreVariablesValid(name, null,
            (_uiGameObject, nameof(_uiGameObject)),
            (_backgroundImage, nameof(_backgroundImage)),

            (_audioSource, nameof(_audioSource)),

            (_timeAcceleratingSFX, nameof(_timeAcceleratingSFX))
        )) return;

        // -- Handling events -- //

        EventHandler.OnMaximumRemainingResponseTimeChangedEvent += OnMaximumRemainingResponseTimeChanged;

        // -- Changing UI color -- //

        _backgroundImage.color = new(
            LevelDifficultyProperties.DIFFICULTY_COLORS[LevelDifficulty.Progressive].X,
            LevelDifficultyProperties.DIFFICULTY_COLORS[LevelDifficulty.Progressive].Y,
            LevelDifficultyProperties.DIFFICULTY_COLORS[LevelDifficulty.Progressive].Z,
            _backgroundImageTransparency
        );

        // -- Loading the SFX -- //

        _audioSource.clip = _timeAcceleratingSFX;
    }

    void OnDestroy()
    {
        EventHandler.OnMaximumRemainingResponseTimeChangedEvent -= OnMaximumRemainingResponseTimeChanged;
    }


    void OnMaximumRemainingResponseTimeChanged(float p_maximumRemainingTimeInSeconds)
    {
        // If it's the first time we modify the value.
        // That happens when the Level starts.
        // We don't want to trigger the feedback in that case.
        if (_maximumRemainingTime == -1)
        {
            if (_isDebugModeOn)
                Debug.Log($"DEBUG: [{GetType().Name}] The maximum remaining time has changed for the first time during this Level. The feedback will not be triggered. Returning.");

            _maximumRemainingTime = p_maximumRemainingTimeInSeconds;
            return;
        }

        if (_maximumRemainingTime == p_maximumRemainingTimeInSeconds)
        {
            if (_isDebugModeOn)
                Debug.Log($"DEBUG: [{GetType().Name}] The maximum remaining time has been modified during the same Level but with the same value as before. The feedback will not be triggered. Returning.");

            _maximumRemainingTime = p_maximumRemainingTimeInSeconds;
            return;
        }

        _maximumRemainingTime = p_maximumRemainingTimeInSeconds;

        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] The maximum remaining time has been changed during the same Level. Starting to do the associated feedback.");

        _audioSource.Play();

        if (_isBlinking)
            StartCoroutine(Blink());
    }


    IEnumerator Blink()
    {
        _uiGameObject.SetActive(true);

        // We don't need to implement anything regarding the game being paused.
        // That's because when we pause the game the Time.timeScale property is set to 0.
        // That means that because WaitForSeconds is dependent on the Time.timeScale property, he will wait for the property to be set back to 1 to continue his countdown.
        yield return new WaitForSeconds(_blinkTimeInSeconds);

        _uiGameObject.SetActive(false);
    }
}
