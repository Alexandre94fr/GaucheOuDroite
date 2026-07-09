using TMPro;
using UnityEngine;

using VariableCheckerPackage;


public class DirectionText : MonoBehaviour
{
    [Header("Internal references:")]
    [SerializeField] TextMeshProUGUI _directionText;


    string _currentDirectionText = "";


    void Start()
    {
        // -- Class properties verifications -- //

        if (!VariablesChecker.AreVariablesValid(name, null,
            (_directionText, nameof(_directionText))
        )) return;

        // -- Handling events -- //

        EventHandler.OnNextLevelDirectionComputedEvent += OnNextLevelDirectionComputed;
        PauseManager.OnPauseEvent += OnPause;
    }

    void OnDestroy()
    {
        EventHandler.OnNextLevelDirectionComputedEvent -= OnNextLevelDirectionComputed;
        PauseManager.OnPauseEvent -= OnPause;
    }


    void OnNextLevelDirectionComputed(DirectionProperties.Direction p_direction)
    {
        _currentDirectionText = DirectionProperties.DIRECTIONS_IN_FRENCH[p_direction] + " !";

        _directionText.text = _currentDirectionText;
    }


    void OnPause(bool p_isGamePaused)
    {
        // We do this to avoid the player from using the pause to cheat by pausing at every direction question.

        if (p_isGamePaused)
        {
            _directionText.text = "";
        }
        else
        {
            _directionText.text = _currentDirectionText;
        }
    }
}