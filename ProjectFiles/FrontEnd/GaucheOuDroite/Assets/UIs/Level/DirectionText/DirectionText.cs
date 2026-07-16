using System.Collections.Generic;
using UnityEngine;
using TMPro;

using VariableCheckerPackage;

using FrontEnd.Data.Game;


public class DirectionText : MonoBehaviour
{
    [Header("Internal references:")]
    [SerializeField] TextMeshProUGUI _directionText;


    string _currentDirectionText = "";


    Dictionary<DirectionProperties.Direction, string> DIRECTIONS = new();


    void Start()
    {
        // -- Class properties verifications -- //

        if (!VariablesChecker.AreVariablesValid(name, null,
            (_directionText, nameof(_directionText))
        )) return;

        // -- Setting the different properties to the right language -- //

        switch (GameDataManager.Instance.GameLanguage)
        {
            case GameLanguage.French:

                DIRECTIONS = DirectionProperties.DIRECTIONS_IN_FRENCH;

                break;

            case GameLanguage.English:

                DIRECTIONS = DirectionProperties.DIRECTIONS_IN_ENGLISH;

                break;

            default:
                Debug.LogError($"ERROR: [{GetType().Name}] There is no case planned in the switch for '{GameDataManager.Instance.GameLanguage}'. Using the Level name inside the DataBase.");
                break;
        }

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
        _currentDirectionText = DIRECTIONS[p_direction] + " !";

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