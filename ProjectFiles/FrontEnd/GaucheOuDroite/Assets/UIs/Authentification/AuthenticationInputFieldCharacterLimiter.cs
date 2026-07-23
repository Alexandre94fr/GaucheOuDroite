using UnityEngine;
using TMPro;

using VariableCheckerPackage;

using Shared.Constants;


public class AuthenticationInputFieldCharacterLimiter : MonoBehaviour
{
    [Header("----- DEBUG -----")]
    [SerializeField] bool _isDebugModeOn;


    [Header("Internal references:")]
    [SerializeField] TMP_InputField _usernameInputField;
    [SerializeField] TMP_InputField _passwordInputField;


    void Start()
    {
        // -- Class properties verifications -- //

        if (!VariablesChecker.AreVariablesValid(name, null,
            (_usernameInputField, nameof(_usernameInputField)),
            (_passwordInputField, nameof(_passwordInputField))
        )) return;

        // -- Updating the authentication inputs fields -- //

        SetInputFieldCharacterLimit(_usernameInputField, AuthenticationProperties.USERNAME_MAXIMUM_LENGHT);
        SetInputFieldCharacterLimit(_passwordInputField, AuthenticationProperties.PASSWORD_MAXIMUM_LENGHT);
    }


    void SetInputFieldCharacterLimit(TMP_InputField p_inputField, int p_newCharacterLimit)
    {
        if (p_newCharacterLimit < 0)
        {
            Debug.LogWarning($"WARNING: [{GetType().Name}] Can't set the character limit of the '{p_inputField.name}' InputField to a negative value. Returning.");
            return;
        }

        p_inputField.characterLimit = p_newCharacterLimit;

        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Successfully set the character limit of the '{p_inputField.name}' InputField to {p_newCharacterLimit}.");
    }
}