using UnityEngine;
using UnityEngine.UI;
using TMPro;

using VariableCheckerPackage;

using FrontEnd.Data.Game;
using FrontEnd.Data.User;


public class GameRules : MonoBehaviour
{
    [Header("External references:")]
    [SerializeField] Button _gameRulesButton;
    [SerializeField] TextMeshProUGUI _gameRulesButtonText;

    [Header("Internal references:")]
    [SerializeField] GameObject _gameRulesUIGameObject;

    [Space]
    [SerializeField] TextMeshProUGUI _titleText;

    [Space]
    [SerializeField] TextMeshProUGUI _gameGoalTitleText;
    [SerializeField] TextMeshProUGUI _gameGoalText;

    [Space]
    [SerializeField] TextMeshProUGUI _scoringSystemTitleText;
    [SerializeField] TextMeshProUGUI _scoringSystemText;

    [Space]
    [SerializeField] TextMeshProUGUI _closeButtonText;


    [Header("Properties:")]
    [SerializeField] int _neededUnlockedLevelToStopUIShow = 2;


    // Localization
    string GAME_RULES_BUTTON_TEXT = null;

    string TITLE_TEXT = null;

    string GAME_GOAL_TITLE_TEXT = null;
    string GAME_GOAL_TEXT = null;

    string SCORING_SYSTEM_TITLE_TEXT = null;
    string SCORING_SYSTEM_TEXT = null;

    string CLOSE_BUTTON_TEXT = null;


    void Start()
    {
        // -- Class properties verifications -- //

        if (!VariablesChecker.AreVariablesValid(name, null,
            (_gameRulesButton, nameof(_gameRulesButton)),
            (_gameRulesButtonText, nameof(_gameRulesButtonText)),

            (_gameRulesUIGameObject, nameof(_gameRulesUIGameObject)),

            (_titleText, nameof(_titleText)),

            (_gameGoalTitleText, nameof(_gameGoalTitleText)),
            (_gameGoalText, nameof(_gameGoalText)),

            (_scoringSystemTitleText, nameof(_scoringSystemTitleText)),
            (_scoringSystemText, nameof(_scoringSystemText)),

            (_closeButtonText, nameof(_closeButtonText))
        )) return;

        if (_neededUnlockedLevelToStopUIShow <= 0)
        {
            Debug.LogWarning($"WARNING: [{GetType().Name}] The '{nameof(_neededUnlockedLevelToStopUIShow)}' property is equal or inferior to 0. Returning.");
            return;
        }

        // -- Setting the different properties to the right language -- //

        switch (GameDataManager.Instance.GameLanguage)
        {
            case GameLanguage.French:

                GAME_RULES_BUTTON_TEXT = GameRulesProperties.GAME_RULES_BUTTON_TEXT_IN_FRENCH;

                TITLE_TEXT = GameRulesProperties.TITLE_TEXT_IN_FRENCH;

                GAME_GOAL_TITLE_TEXT = GameRulesProperties.GAME_GOAL_TITLE_TEXT_IN_FRENCH;
                GAME_GOAL_TEXT = GameRulesProperties.GAME_GOAL_TEXT_IN_FRENCH;

                SCORING_SYSTEM_TITLE_TEXT = GameRulesProperties.SCORING_SYSTEM_TITLE_TEXT_IN_FRENCH;
                SCORING_SYSTEM_TEXT = GameRulesProperties.SCORING_SYSTEM_TEXT_IN_FRENCH;

                CLOSE_BUTTON_TEXT = GameRulesProperties.CLOSE_BUTTON_TEXT_IN_FRENCH;

                break;

            case GameLanguage.English:

                GAME_RULES_BUTTON_TEXT = GameRulesProperties.GAME_RULES_BUTTON_TEXT_IN_ENGLISH;

                TITLE_TEXT = GameRulesProperties.TITLE_TEXT_IN_ENGLISH;

                GAME_GOAL_TITLE_TEXT = GameRulesProperties.GAME_GOAL_TITLE_TEXT_IN_ENGLISH;
                GAME_GOAL_TEXT = GameRulesProperties.GAME_GOAL_TEXT_IN_ENGLISH;

                SCORING_SYSTEM_TITLE_TEXT = GameRulesProperties.SCORING_SYSTEM_TITLE_TEXT_IN_ENGLISH;
                SCORING_SYSTEM_TEXT = GameRulesProperties.SCORING_SYSTEM_TEXT_IN_ENGLISH;

                CLOSE_BUTTON_TEXT = GameRulesProperties.CLOSE_BUTTON_TEXT_IN_ENGLISH;

                break;

            default:
                Debug.LogError($"ERROR: [{GetType().Name}] There is no case planned in the switch for '{GameDataManager.Instance.GameLanguage}'. Returning.");
                return;
        }

        // -- Setting up the right texts -- //

        InitializeTexts();

        // -- Showing the UI if the player is new -- //

        if (!UserDataManager.Instance.TryGetLevelProgression(_neededUnlockedLevelToStopUIShow, out LevelProgression levelProgression))
        {
            Debug.LogWarning($"WARNING: [{GetType().Name}] Failed to get the {nameof(LevelProgression)} {_neededUnlockedLevelToStopUIShow}. There is no {nameof(LevelProgression)} associated with the Level Id: {_neededUnlockedLevelToStopUIShow}. Showing shortcut tutorial failed. Returning.");
            return;
        }

        if (levelProgression.IsUnlocked == false)
        {
            _gameRulesUIGameObject.SetActive(true);

            _gameRulesButton.interactable = false;
        }
    }


    void InitializeTexts()
    {
        _gameRulesButtonText.text = GAME_RULES_BUTTON_TEXT;

        _titleText.text = TITLE_TEXT;

        _gameGoalTitleText.text = GAME_GOAL_TITLE_TEXT;
        _gameGoalText.text = GAME_GOAL_TEXT;

        _scoringSystemTitleText.text = SCORING_SYSTEM_TITLE_TEXT;
        _scoringSystemText.text = SCORING_SYSTEM_TEXT;

        _closeButtonText.text = CLOSE_BUTTON_TEXT;
    }

    public void OnButtonPressed()
    {
        _gameRulesUIGameObject.SetActive(false);

        _gameRulesButton.interactable = true;
    }
}