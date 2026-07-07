using FrontEnd.Data.Game;
using TMPro;
using UnityEngine;

using VariableCheckerPackage;


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


    void OnLevelWon(Level p_levelProperties, bool p_hasNextLevelBeenUnlocked, bool p_isNextLevelAlreadyUnlocked, int p_score, bool p_isPreviousBestScoreBeaten)
    {
        _gameOverUIGameObject.SetActive(true);

        _levelNameText.text = p_levelProperties.Name;
        _titleText.text = "Terminé !";

        _scoreBar.UpdateVisuals(p_levelProperties, p_score);

        _isNextLevelUnclockedText.enabled = p_hasNextLevelBeenUnlocked;

        _nextLevelButton.SetButtonInteractability(p_hasNextLevelBeenUnlocked || p_isNextLevelAlreadyUnlocked);
    }

    void OnLevelLost(Level p_levelProperties, bool p_hasNextLevelBeenUnlocked, bool p_isNextLevelAlreadyUnlocked, int p_score, bool p_isPreviousBestScoreBeaten)
    {
        _gameOverUIGameObject.SetActive(true);

        _levelNameText.text = p_levelProperties.Name;
        _titleText.text = "Échoué !";

        _scoreBar.UpdateVisuals(p_levelProperties, p_score);

        _isNextLevelUnclockedText.enabled = p_hasNextLevelBeenUnlocked;

        _nextLevelButton.SetButtonInteractability(p_hasNextLevelBeenUnlocked || p_isNextLevelAlreadyUnlocked);
    }
}