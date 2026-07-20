using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
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

        EventHandler.OnDirectionChoiceInputEvent += OnDirectionChoiceInput;
        PauseManager.OnPauseEvent += OnPause;
    }

    void OnDestroy()
    {
        EventHandler.OnDirectionChoiceInputEvent -= OnDirectionChoiceInput;
        PauseManager.OnPauseEvent -= OnPause;
    }



    IEnumerator SimulatePress()
    {
        PointerEventData eventData = new(EventSystem.current);

        _button.OnPointerDown(eventData);

        yield return new WaitForSeconds(0.1f); 

        _button.OnPointerUp(eventData);
    }

    void OnDirectionChoiceInput(DirectionProperties.Direction p_direction)
    {
        if (p_direction != _buttonDirection) 
            return;

        StartCoroutine(SimulatePress());
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