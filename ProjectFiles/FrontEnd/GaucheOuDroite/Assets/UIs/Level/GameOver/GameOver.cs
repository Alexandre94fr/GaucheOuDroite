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


    void Start()
    {
        // -- Class properties verifications -- //

        if (!VariablesChecker.AreVariablesValid(name, null,
            (_gameOverUIGameObject, nameof(_gameOverUIGameObject)),

            (_levelNameText, nameof(_levelNameText)),
            (_titleText, nameof(_titleText))
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


    void OnLevelWon(Level p_levelProperties, bool p_isNextLevelUnlocked, int p_score, bool p_isPreviousBestScoreBeaten)
    {
        _gameOverUIGameObject.SetActive(true);

        _levelNameText.text = p_levelProperties.Name;
        _titleText.text = "Terminé !";
    }

    void OnLevelLost(Level p_levelProperties, bool p_isNextLevelUnlocked, int p_score, bool p_isPreviousBestScoreBeaten)
    {
        _gameOverUIGameObject.SetActive(true);

        _levelNameText.text = p_levelProperties.Name;
        _titleText.text = "Échoué !";
    }
}
