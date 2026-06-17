using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputsReceiver : MonoBehaviour
{
    [Header("----- DEBUG -----")]
    public bool IsDebugModeOn;

    public void OnDirectionChoiceInput(InputAction.CallbackContext p_callbackContext)
    {
        if (IsDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] DirectionChoice input interactation detected. OnPauseInput() method called.");

        if (!p_callbackContext.performed)
            return;

        if (IsDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] DirectionChoice input pressed.");

        // The ReadValue<Single>() should return -1 or 1
        float directionNumber = p_callbackContext.ReadValue<Single>();
        
        // Converting the 'directionNumber' into a boolean
        bool isChoosenDirectionRight = true;

        if (directionNumber == -1)
        {
            isChoosenDirectionRight = false;
        }
        else if (directionNumber == 1)
        {
            isChoosenDirectionRight = true;
        }
        else
        {
            Debug.LogWarning($"WARNING: [{GetType().Name}] Received a direction number not equal to -1 or 1, got: '{directionNumber}'. Returning");
            return;
        }


        // Firing the Event
        if (IsDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Trying firing the OnDirectionChoiceInputEvent: EventHandler.OnDirectionChoiceInputEvent?.Invoke({isChoosenDirectionRight}).");

        EventHandler.OnDirectionChoiceInputEvent?.Invoke(isChoosenDirectionRight);
    }

    public void OnPauseInput(InputAction.CallbackContext p_callbackContext)
    {
        if (IsDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Pause input interactation detected. OnPauseInput() method called.");

        if (!p_callbackContext.performed)
            return;

        if (IsDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Pause input pressed.");


        // Firing the Event
        if (IsDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Trying firing the OnPauseInputEvent: EventHandler.OnPauseInputEvent?.Invoke().");

        EventHandler.OnPauseInputEvent?.Invoke();
    }
}