using TMPro;
using UnityEngine;

using VariableCheckerPackage;


public class DirectionText : MonoBehaviour
{
    [Header("Internal references:")]
    [SerializeField] TextMeshProUGUI _directionText;


    void Start()
    {
        // -- Class properties verifications -- //

        if (!VariablesChecker.AreVariablesValid(name, null,
            (_directionText, nameof(_directionText))
        )) return;

        // -- Handling events -- //

        EventHandler.OnNextLevelDirectionComputedEvent += OnNextLevelDirectionComputed;
    }

    void OnDestroy()
    {
        EventHandler.OnNextLevelDirectionComputedEvent -= OnNextLevelDirectionComputed;
    }


    void OnNextLevelDirectionComputed(DirectionProperties.Direction p_direction)
    {
        _directionText.text = DirectionProperties.DIRECTIONS_IN_FRENCH[p_direction] + " !";
    }
}