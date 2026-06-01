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
            Debug.Log("DEBUG: DirectionChoice input interactation detected. OnPauseInput() method called.");

        if (!p_callbackContext.performed)
            return;

        if (IsDebugModeOn)
            Debug.Log("DEBUG: DirectionChoice input pressed.");

        //var e = p_callbackContext.ReadValue<Single>();
        // The ReadValue<Single>() will return -1 or 1

        // TODO: Must fire the OnDirectionChoice Action
    }

    public void OnPauseInput(InputAction.CallbackContext p_callbackContext)
    {
        if (IsDebugModeOn)
            Debug.Log("DEBUG: Pause input interactation detected. OnPauseInput() method called.");

        if (!p_callbackContext.performed)
            return;

        if (IsDebugModeOn)
            Debug.Log("DEBUG: Pause input pressed.");

        // TODO: Must fire the OnPause Action
    }
}