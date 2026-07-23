using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

using VariableCheckerPackage;


public class AuthenticationNavigation : MonoBehaviour
{
    [Header("----- DEBUG -----")]
    [SerializeField] bool _isDebugModeOn;


    [Header("Internal references:")]
    [SerializeField] TMP_InputField _usernameInputField;
    [SerializeField] TMP_InputField _passwordInputField;

    [Space]
    [SerializeField] Button _authenticationButton;

    [Space]
    [SerializeField] Button _switchAuthenticationModeButton;


    readonly Dictionary<Selectable, Selectable> _navigationDictionary = new(3);


    private void Start()
    {
        // -- Class properties verifications -- //

        if (!VariablesChecker.AreVariablesValid(name, null,
            (_usernameInputField, nameof(_usernameInputField)),
            (_passwordInputField, nameof(_passwordInputField)),

            (_authenticationButton, nameof(_authenticationButton)),

            (_switchAuthenticationModeButton, nameof(_switchAuthenticationModeButton))
        )) return;

        // -- Handling events -- //
        
        EventHandler.OnAuthenticationNavigationInputEvent += OnAuthenticationNavigationInput;
        EventHandler.OnAuthenticationValidationInputEvent += OnAuthenticationValidationInput;

        // -- Initializing navigation dictionary -- //

        InitializeNavigationDictionary();
    }

    void OnDestroy()
    {
        EventHandler.OnAuthenticationNavigationInputEvent -= OnAuthenticationNavigationInput;
        EventHandler.OnAuthenticationValidationInputEvent -= OnAuthenticationValidationInput;
    }


    void InitializeNavigationDictionary()
    {
        _navigationDictionary.Clear();

        // Note: The dictionary MUST always loop on itself. The last element must refer to the first element.

        _navigationDictionary.Add(_usernameInputField, _passwordInputField);
        _navigationDictionary.Add(_passwordInputField, _authenticationButton);
        _navigationDictionary.Add(_authenticationButton, _switchAuthenticationModeButton);
        _navigationDictionary.Add(_switchAuthenticationModeButton, _usernameInputField);
    }


    void OnAuthenticationNavigationInput()
    {
        GameObject selectedGameObject = EventSystem.current.currentSelectedGameObject;

        if (selectedGameObject == null)
        {
            if (_isDebugModeOn)
                Debug.Log($"DEBUG: [{GetType().Name}] There is no GameObject currently selected by the player. Returning.");

            return;
        }

        if (!selectedGameObject.TryGetComponent(out Selectable selectedSelectable))
        {
            Debug.LogWarning($"WARNING: [{GetType().Name}] Failed to get the Selectable component from the '{selectedGameObject.name}' GameObject. Returning.");
            return;
        }

        if (!_navigationDictionary.TryGetValue(selectedSelectable, out Selectable newSelectedObject))
        {
            Debug.LogWarning($"WARNING: [{GetType().Name}] Failed to get a Selectable in the '{nameof(_navigationDictionary)}' Dictionary using the '{selectedSelectable.name}' Key. That Key doesn't exist. Returning.");
            return;
        }

        newSelectedObject.Select();

        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Successfully selected the next authentification element: {selectedSelectable.name} -> {newSelectedObject.name}.");
    }

    void OnAuthenticationValidationInput()
    {
        GameObject selectedGameObject = EventSystem.current.currentSelectedGameObject;

        if (selectedGameObject == null)
        {
            if (_isDebugModeOn)
                Debug.Log($"DEBUG: [{GetType().Name}] There is no GameObject currently selected by the player. Returning.");

            return;
        }

        if (!selectedGameObject.TryGetComponent(out Selectable selectedSelectable))
        {
            Debug.LogWarning($"WARNING: [{GetType().Name}] Failed to get the Selectable component from the '{selectedGameObject.name}' GameObject. Returning.");
            return;
        }

        // Note: We do this check to avoid starting an authentication request AND doing the logic of the currently selected GameObject (potentially a button)

        if (selectedSelectable != _usernameInputField && selectedSelectable != _passwordInputField)
        {
            if (_isDebugModeOn)
                Debug.Log($"DEBUG: [{GetType().Name}] The currently selection GameObject '{selectedGameObject.name}' is not the '{_usernameInputField.name}' or '{_passwordInputField.name}' GameObject. Returning.");

            return;
        }

        // To tell the button to do the visual effects when being pressed.
        _authenticationButton.OnSubmit(new BaseEventData(EventSystem.current));

        // To tell the button to do the logic when being pressed.
        _authenticationButton.onClick?.Invoke();

        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Successfully told the authentication button '{_authenticationButton.name}' to be pressed. He should start an authentication request.");
    }
}