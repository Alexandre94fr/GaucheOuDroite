using UnityEngine;
using UnityEngine.UI;

using VariableCheckerPackage;


public class PauseButton : MonoBehaviour
{
    [Header("Internal references:")]
    [SerializeField] Image _pauseIconImage;

    [Header("Properties:")]
    [SerializeField] Sprite _pauseIcon;
    [SerializeField] Sprite _resumeIcon;


    void Start()
    {
        // -- Class properties verifications -- //

        if (!VariablesChecker.AreVariablesValid(name, null,
            (_pauseIconImage, nameof(_pauseIconImage)),

            (_pauseIcon, nameof(_pauseIcon)),
            (_resumeIcon, nameof(_resumeIcon))

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
        if (p_isGamePaused)
        {
            _pauseIconImage.sprite = _resumeIcon;
        }
        else
        {
            _pauseIconImage.sprite = _pauseIcon;
        }
    }
}