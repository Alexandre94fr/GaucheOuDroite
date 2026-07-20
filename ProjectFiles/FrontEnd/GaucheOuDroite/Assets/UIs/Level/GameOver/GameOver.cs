using UnityEngine;
using TMPro;

using VariableCheckerPackage;

using FrontEnd.Data.Game;


public class GameOver : MonoBehaviour
{
    [Header("Internal references:")]
    [SerializeField] GameObject _gameOverUIGameObject;

    [Space]
    [SerializeField] TextMeshProUGUI _levelNameText;
    [SerializeField] TextMeshProUGUI _titleText;

    [Space]
    [SerializeField] ScoreBar _scoreBar;

    [Space]
    [SerializeField] TextMeshProUGUI _isNextLevelUnclockedText;

    [Space]
    [SerializeField] NextLevelButton _nextLevelButton;


    void Start()
    {
        // -- Class properties verifications -- //

        if (!VariablesChecker.AreVariablesValid(name, null,
            (_gameOverUIGameObject, nameof(_gameOverUIGameObject)),

            (_levelNameText, nameof(_levelNameText)),
            (_titleText, nameof(_titleText)),

            (_scoreBar, nameof(_scoreBar)),

            (_isNextLevelUnclockedText, nameof(_isNextLevelUnclockedText)),

            (_nextLevelButton, nameof(_nextLevelButton))
        )) return;

        // -- Handling events -- //

        EventHandler.OnLevelWonEvent += OnLevelWon;
        EventHandler.OnLevelLostEvent += OnLevelLost;
    }

    void OnDestroy()
    {
        EventHandler.OnLevelWonEvent -= OnLevelWon;
        EventHandler.OnLevelLostEvent -= OnLevelLost;
    }


    void UpdateTitleText(bool p_isLevelWon)
    {
        string titleText;

        if (p_isLevelWon)
        {
            // Doing this conversion this way is bad practice, but because the English version of the game is not subject to being clean and must be done quickly, it's not an issue here.

            switch (GameDataManager.Instance.GameLanguage)
            {
                case GameLanguage.French:

                    titleText = "Terminé !";

                    break;

                case GameLanguage.English:

                    titleText = "Finished!";

                    break;

                default:
                    Debug.LogError($"ERROR: [{GetType().Name}] There is no case planned in the switch for '{GameDataManager.Instance.GameLanguage}'. Using the English translation.");
                    titleText = "Finished!";
                    break;
            }

            _titleText.text = titleText;

            return;
        }

        // Doing this conversion this way is bad practice, but because the English version of the game is not subject to being clean and must be done quickly, it's not an issue here.

        switch (GameDataManager.Instance.GameLanguage)
        {
            case GameLanguage.French:

                titleText = "Échoué !";

                break;

            case GameLanguage.English:

                titleText = "Failed!";

                break;

            default:
                Debug.LogError($"ERROR: [{GetType().Name}] There is no case planned in the switch for '{GameDataManager.Instance.GameLanguage}'. Using the English translation.");
                titleText = "Failed!";
                break;
        }

        _titleText.text = titleText;
    }

    void OnLevelEnd(bool p_isLevelWon, Level p_levelProperties, bool p_hasNextLevelBeenUnlocked, bool p_isNextLevelAlreadyUnlocked, int p_score, bool p_isPreviousBestScoreBeaten)
    {
        _gameOverUIGameObject.SetActive(true);

        _levelNameText.text = p_levelProperties.Name;

        UpdateTitleText(p_isLevelWon);

        _scoreBar.UpdateVisuals(p_levelProperties, p_score);

        _isNextLevelUnclockedText.enabled = p_hasNextLevelBeenUnlocked;

        _nextLevelButton.SetButtonInteractability(p_hasNextLevelBeenUnlocked || p_isNextLevelAlreadyUnlocked);
    }

    void OnLevelWon(Level p_levelProperties, bool p_hasNextLevelBeenUnlocked, bool p_isNextLevelAlreadyUnlocked, int p_score, bool p_isPreviousBestScoreBeaten)
    {
        OnLevelEnd(true, p_levelProperties, p_hasNextLevelBeenUnlocked, p_isNextLevelAlreadyUnlocked, p_score, p_isPreviousBestScoreBeaten);
    }

    void OnLevelLost(Level p_levelProperties, bool p_hasNextLevelBeenUnlocked, bool p_isNextLevelAlreadyUnlocked, int p_score, bool p_isPreviousBestScoreBeaten)
    {
        OnLevelEnd(false, p_levelProperties, p_hasNextLevelBeenUnlocked, p_isNextLevelAlreadyUnlocked, p_score, p_isPreviousBestScoreBeaten);
    }
}