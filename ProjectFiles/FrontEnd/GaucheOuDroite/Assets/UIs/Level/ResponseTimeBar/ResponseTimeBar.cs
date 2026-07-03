using UnityEngine;
using UnityEngine.UI;

using VariableCheckerPackage;


public class ResponseTimeBar : MonoBehaviour
{
    [Header("Internal references:")]
    [SerializeField] Image _fillImage;


    float _maximumRemainingTime = -1;


    void Start()
    {
        // -- Class properties verifications -- //

        if (!VariablesChecker.AreVariablesValid(name, null,
            (_fillImage, nameof(_fillImage))
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