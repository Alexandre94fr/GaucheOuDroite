using UnityEngine;

using VariableCheckerPackage;

using FrontEnd.Data.User;


public class ShortcutTutorial : MonoBehaviour
{
    [Header("Internal references:")]
    [SerializeField] GameObject _gameOverUIGameObject;


    [Header("Properties:")]
    [SerializeField] int _neededUnlockedLevelToStopTutorialAutoShow = 2;


    void Start()
    {
        // -- Class properties verifications -- //

        if (!VariablesChecker.AreVariablesValid(name, null,
            (_gameOverUIGameObject, nameof(_gameOverUIGameObject))
        )) return;

        if (_neededUnlockedLevelToStopTutorialAutoShow <= 0)
        {
            Debug.LogWarning($"WARNING: [{GetType().Name}] The '{nameof(_neededUnlockedLevelToStopTutorialAutoShow)}' property is equal or inferior to 0. Returning.");
            return;
        }

        // -- Showing the tutorial if the player is new -- //

        if (!UserDataManager.Instance.TryGetLevelProgression(_neededUnlockedLevelToStopTutorialAutoShow, out LevelProgression levelProgression))
        {
            Debug.LogWarning($"WARNING: [{GetType().Name}] Failed to get the {nameof(LevelProgression)} {_neededUnlockedLevelToStopTutorialAutoShow}. There is no {nameof(LevelProgression)} associated with the Level Id: {_neededUnlockedLevelToStopTutorialAutoShow}. Showing shortcut tutorial failed. Returning.");
            return;
        }

        if (levelProgression.IsUnlocked == false)
            _gameOverUIGameObject.SetActive(true);
    }
}