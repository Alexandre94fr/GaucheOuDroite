using UnityEngine;

using VariableCheckerPackage;


public class PauseMenu : MonoBehaviour
{
    [Header("Internal references:")]
    [SerializeField] GameObject _gameOverUIGameObject;


    void Start()
    {
        // -- Class properties verifications -- //

        if (!VariablesChecker.AreVariablesValid(name, null,
            (_gameOverUIGameObject, nameof(_gameOverUIGameObject))
        )) return;

        // -- Handling events -- //

        PauseManager.OnPauseEvent += OnPause;
    }

    void OnDestroy()
    {
        PauseManager.OnPauseEvent -= OnPause;
    }


    public void OnButtonPressed()
    {
        // We do this to trigger the PauseManager the same way the player could press the Escape key to pause the game.

        EventHandler.OnPauseInputEvent?.Invoke();

        // The PauseManager will invoke the OnPauseEvent, the PauseButton, PauseMenu, and other scripts listen to it.
    }


    void OnPause(bool p_isGamePaused)
    {
        UpdateVisuals(p_isGamePaused);
    }

    void UpdateVisuals(bool p_isGamePaused)
    {
        _gameOverUIGameObject.SetActive(p_isGamePaused);
    }
}