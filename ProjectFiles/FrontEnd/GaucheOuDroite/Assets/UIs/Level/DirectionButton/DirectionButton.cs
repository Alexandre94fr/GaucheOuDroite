using UnityEngine;
using UnityEngine.UI;

using VariableCheckerPackage;


public class DirectionButton : MonoBehaviour
{
    [Header("Internal references:")]
    [SerializeField] Button _button;

    [Header("Properties:")]
    [SerializeField] DirectionProperties.Direction _buttonDirection;


    void Start()
    {
        // -- Class properties verifications -- //

        if (!VariablesChecker.AreVariablesValid(name, null,
            (_button, nameof(_button))
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
        EventHandler.OnDirectionChoiceInputEvent?.Invoke(_buttonDirection);
    }


    void OnPause(bool p_isGamePaused)
    {
        _button.interactable = !p_isGamePaused;
    }
}