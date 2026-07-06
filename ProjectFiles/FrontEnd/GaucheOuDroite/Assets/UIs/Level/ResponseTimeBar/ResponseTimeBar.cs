using UnityEngine;
using UnityEngine.UI;

using VariableCheckerPackage;


public class ResponseTimeBar : MonoBehaviour
{
    [Header("Internal references:")]
    [SerializeField] Image _fillImage;

    [Space]
    [SerializeField] Image _perfectResponseTimeAreaFillImage;


    float _maximumRemainingTime = -1;


    void Start()
    {
        // -- Class properties verifications -- //

        if (!VariablesChecker.AreVariablesValid(name, null,
            (_fillImage, nameof(_fillImage)),

            (_perfectResponseTimeAreaFillImage, nameof(_perfectResponseTimeAreaFillImage))
        )) return;

        // -- Handling events -- //

        EventHandler.OnMaximumRemainingResponseTimeChangedEvent += OnMaximumRemainingResponseTimeChanged;
        EventHandler.OnRemainingResponseTimeChangedEvent += OnRemainingResponseTimeChanged;
    }

    void OnDestroy()
    {
        EventHandler.OnMaximumRemainingResponseTimeChangedEvent -= OnMaximumRemainingResponseTimeChanged;
        EventHandler.OnRemainingResponseTimeChangedEvent -= OnRemainingResponseTimeChanged;
    }


    void OnMaximumRemainingResponseTimeChanged(float p_maximumRemainingTimeInSeconds)
    {
        _maximumRemainingTime = p_maximumRemainingTimeInSeconds;

        _perfectResponseTimeAreaFillImage.fillAmount = ScoreProperties.MINIMUM_REMAINING_TIME_IN_SECONDS_FOR_MAXIMUM_SCORE / _maximumRemainingTime;
    }

    void OnRemainingResponseTimeChanged(float p_remainingTimeInSeconds)
    {
        if (_maximumRemainingTime < 0)
        {
            Debug.LogWarning($"WARNING: [{GetType().Name}] The '{nameof(_maximumRemainingTime)}' has not been changed. Returning.");
            return;
        }

        _fillImage.fillAmount = p_remainingTimeInSeconds / _maximumRemainingTime;
    }
}