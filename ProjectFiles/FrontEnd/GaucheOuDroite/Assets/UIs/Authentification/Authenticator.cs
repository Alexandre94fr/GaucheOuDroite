using System.Linq;
using System.Collections;
using TMPro;
using UnityEngine;

using VariableCheckerPackage;

// For networking (TEMP TODO: Remove)
using System.Text;
using UnityEngine.Networking;

using Shared.Constants;
using Shared.DTOs;

using Newtonsoft.Json;

public class Authenticator : MonoBehaviour
{
    [Header("----- DEBUG -----")]
    [SerializeField] bool _isDebugModeOn;


    [Header("External references:")]
    [SerializeField] TextMeshProUGUI _feedbackText;

    [Header("Properties:")]
    [SerializeField] AuthenticationProperties.AuthenticationMode _authenticationMode = AuthenticationProperties.AuthenticationMode.SignUp;

    string _username = "";
    string _password = "";


    void Start()
    {
        if (!VariablesChecker.AreVariablesValid(name, null,
            (_feedbackText, nameof(_feedbackText))
        )) return;
    }


    void SetAuthenticationValue(string p_newInputFieldValue, bool p_isUsernameValueModified)
    {
        if (_isDebugModeOn)
        {
            if (p_isUsernameValueModified)
                Debug.Log($"DEBUG: [{GetType().Name}] Setting '{nameof(_username)}' variable to: '{p_newInputFieldValue}'.");
            else
                // Doing: new string('*', p_newInputFieldValue.Length), transform the password 'Password123' into '***********', avoiding printing the password.
                Debug.Log($"DEBUG: [{GetType().Name}] Setting '{nameof(_password)}' variable to: '{new string('*', p_newInputFieldValue.Length)}'.");
        }
        
        if (p_isUsernameValueModified)
            _username = p_newInputFieldValue;
        else
            _password = p_newInputFieldValue;
    }

    #region Input field callbacks

    #region OnInputFieldValueChanged

    public void OnUsernameInputFieldValueChanged(string p_newInputFieldValue)
    {
        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] UsernameInputField's value modification detected. Modifying '{nameof(_username)}' variable.");

        SetAuthenticationValue(p_newInputFieldValue, true);
    }

    public void OnPasswordInputFieldValueChanged(string p_newInputFieldValue)
    {
        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] PasswordInputField's value modification detected. Modifying '{nameof(_password)}' variable.");

        SetAuthenticationValue(p_newInputFieldValue, false);
    }

    #endregion

    #region OnInputFieldEndEdit

    public void OnUsernameInputFieldEndEdit(string p_newInputFieldValue)
    {
        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] UsernameInputField's value modification end detected. Modifying '{nameof(_username)}' variable.");

        SetAuthenticationValue(p_newInputFieldValue, true);
    }

    public void OnPasswordInputFieldEndEdit(string p_newInputFieldValue)
    {
        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] PasswordInputField's value modification end detected. Modifying '{nameof(_password)}' variable.");

        SetAuthenticationValue(p_newInputFieldValue, false);
    }

    #endregion

    #endregion


    #region Authentication value validation

    bool IsUsernameValid(string p_username, out AuthenticationProperties.AuthenticationErrorReasons p_authenticationErrorReason)
    {
        // To silence the linter.
        // Normaly a program will not use the 'p_authenticationErrorReason' if the password is valid.
        p_authenticationErrorReason = default;

        if (string.IsNullOrEmpty(p_username))
        {
            p_authenticationErrorReason = AuthenticationProperties.AuthenticationErrorReasons.UsernameIsEmpty;
            return false;
        }

        if (p_username.Length < AuthenticationProperties.USERNAME_MINIMUM_LENGHT)
        {
            p_authenticationErrorReason = AuthenticationProperties.AuthenticationErrorReasons.UsernameIsTooShort;
            return false;
        }

        if (p_username.Length > AuthenticationProperties.USERNAME_MAXIMUM_LENGHT)
        {
            p_authenticationErrorReason = AuthenticationProperties.AuthenticationErrorReasons.UsernameIsTooLong;
            return false;
        }

        return true;
    }

    bool IsPasswordValid(string p_password, out AuthenticationProperties.AuthenticationErrorReasons p_authenticationErrorReason)
    {
        // To silence the linter.
        // Normaly a program will not use the 'p_authenticationErrorReason' if the password is valid.
        p_authenticationErrorReason = default;

        if (string.IsNullOrEmpty(p_password))
        {
            p_authenticationErrorReason = AuthenticationProperties.AuthenticationErrorReasons.PasswordIsEmpty;
            return false;
        }

        if (p_password.Length < AuthenticationProperties.PASSWORD_MINIMUM_LENGHT)
        {
            p_authenticationErrorReason = AuthenticationProperties.AuthenticationErrorReasons.PasswordIsTooShort;
            return false;
        }

        if (p_password.Length > AuthenticationProperties.PASSWORD_MAXIMUM_LENGHT)
        {
            p_authenticationErrorReason = AuthenticationProperties.AuthenticationErrorReasons.PasswordIsTooLong;
            return false;
        }


        if (p_password.Contains(" "))
        {
            p_authenticationErrorReason = AuthenticationProperties.AuthenticationErrorReasons.PasswordContainAtLeastOneSpaceCharacter;
            return false;
        }


        if (AuthenticationProperties.DOES_PASSWORD_MUST_HAVE_AT_LEAST_ONE_LETTER && !p_password.Any(char.IsLetter))
        {
            p_authenticationErrorReason = AuthenticationProperties.AuthenticationErrorReasons.PasswordDoesNotContainAnyLetters;
            return false;
        }

        if (AuthenticationProperties.DOES_PASSWORD_MUST_HAVE_AT_LEAST_ONE_NUMBER && !p_password.Any(char.IsDigit))
        {
            p_authenticationErrorReason = AuthenticationProperties.AuthenticationErrorReasons.PasswordDoesNotContainAnyNumbers;
            return false;
        }

        if (AuthenticationProperties.DOES_PASSWORD_MUST_HAVE_AT_LEAST_ONE_SPECIAL_CHARACTER && !p_password.Any(c => !char.IsLetterOrDigit(c)))
        {
            p_authenticationErrorReason = AuthenticationProperties.AuthenticationErrorReasons.PasswordDoesNotContainAnySpecialCharacters;
            return false;
        }

        return true;
    }

    #endregion

    string GetErrorMessage(AuthenticationProperties.AuthenticationErrorReasons errorReason)
    {
        string errorMessage = AuthenticationProperties.AUTHENTICATION_ERROR_MESSAGES[errorReason];
        
        if (errorMessage == null)
            errorMessage = AuthenticationProperties.UNKNOWN_ERROR_MESSAGE;

        return errorMessage;
    }

    void DisplayFeedback(string p_message, Color p_color)
    {
        _feedbackText.text = p_message;
        _feedbackText.color = p_color;
        _feedbackText.gameObject.SetActive(true);
    }

    public void OnSendButtonPressed()
    {
        #region Checking if the given authentication values are correct

        AuthenticationProperties.AuthenticationErrorReasons errorReason;

        if (!IsUsernameValid(_username, out errorReason))
        {
            string errorMessage = GetErrorMessage(errorReason);

            // Modifying the UI to tell the player, that his username is incorrect.
            DisplayFeedback(
                errorMessage,
                new(
                    AuthenticationProperties.AUTHENTICATION_ERROR_MESSAGE_COLOR.X,
                    AuthenticationProperties.AUTHENTICATION_ERROR_MESSAGE_COLOR.Y,
                    AuthenticationProperties.AUTHENTICATION_ERROR_MESSAGE_COLOR.Z
                )
            );

            if (_isDebugModeOn)
                Debug.Log($"DEBUG: [{GetType().Name}] The given username '{_username}' is not valid, reason: {errorMessage} Returning");

            return;
        }   

        if (!IsPasswordValid(_password, out errorReason))
        {
            string errorMessage = GetErrorMessage(errorReason);

            // Modifying the UI to tell the player, that his password is incorrect.
            DisplayFeedback(
                errorMessage,
                new(
                    AuthenticationProperties.AUTHENTICATION_ERROR_MESSAGE_COLOR.X,
                    AuthenticationProperties.AUTHENTICATION_ERROR_MESSAGE_COLOR.Y,
                    AuthenticationProperties.AUTHENTICATION_ERROR_MESSAGE_COLOR.Z
                )
            );

            if (_isDebugModeOn)
                Debug.Log($"DEBUG: [{GetType().Name}] The given password '{new string('*', _password.Length)}' is not valid, reason: {errorMessage} Returning");

            return;
        }

        #endregion

        // Modifying the UI to tell the player, that his username and password are correct and that we sent a request to the server.
        string authenticationModeString = AuthenticationProperties.AUTHENTICATION_MODE_IN_FRENCH[_authenticationMode].ToUpper();

        DisplayFeedback(
            $"[{authenticationModeString}]\n{AuthenticationProperties.SUCCESSFUL_AUTHENTICATION_MESSAGE}",
            new(
                AuthenticationProperties.AUTHENTICATION_SUCCESS_MESSAGE_COLOR.X,
                AuthenticationProperties.AUTHENTICATION_SUCCESS_MESSAGE_COLOR.Y,
                AuthenticationProperties.AUTHENTICATION_SUCCESS_MESSAGE_COLOR.Z
            )
        );

        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] The given username and password are correct, sending them to the Authentication API.");

        // Sending these values to the correct Authentication API (in BackEnd)
        StartCoroutine(SendAuthenticationRequestToServer(_authenticationMode));
    }


    IEnumerator SendAuthenticationRequestToServer(AuthenticationProperties.AuthenticationMode p_authenticationMode)
    {
        // Getting and creating the good route and DTO depending of the given AuthenticationMode
        // TODO: Prendre le code ci-dessous et en faire une méthode 
        string route;
        object authenticationDTO;

        switch (p_authenticationMode)
        {
            case AuthenticationProperties.AuthenticationMode.SignUp:

                route = "sign-up";

                authenticationDTO = new SignUpDTO()
                {
                    Username = _username,
                    Password = _password
                };

                break;

            case AuthenticationProperties.AuthenticationMode.LogIn:

                route = "log-in";

                authenticationDTO = new LogInDTO()
                {
                    Username = _username,
                    Password = _password
                };

                break;

            default:
                Debug.LogWarning($"WARNING: [{GetType().Name}] The given '{p_authenticationMode}' AuthenticationProperties.AuthenticationMode is not planned in the switch. Returning.");
                yield break;
        }

        // http://localhost:5131 For http
        // https://localhost:7280 For https

        string url = $"https://localhost:7280/api/authentication/{route}";

        // Converting the DTO in Json
        string authenticationDTOInJson = JsonConvert.SerializeObject(authenticationDTO);

        // Creating the request
        // TODO : Prendre le code ci-dessous et en faire une méthode pour envoyer une requete au serveur
        // Voir bloc-note + (création GameDataManager + PlayerDataManager)
        UnityWebRequest request = new(
            url,
            UnityWebRequest.kHttpVerbPOST // kHttpVerbPOST == "POST", we prefer using this because it's a constant, so no typos risk.
        );

        // Adding the Json to the request
        byte[] body = Encoding.UTF8.GetBytes(authenticationDTOInJson);

        // Will store the data when we send the request to the server
        request.uploadHandler = new UploadHandlerRaw(body);

        // Will store the data when the server will respond
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader(
            "Content-Type",
            "application/json"
        );

        // Sending the request to the BackEnd + Waiting for the request response from the BackEnd to come
        yield return request.SendWebRequest();

        object responseBody = JsonConvert.DeserializeObject(request.downloadHandler.text);

        // Handling not "Ok" case
        if (request.result != UnityWebRequest.Result.Success)
        {
            // TODO: The 'ProtocolError' is the not an ERROR, can be like: 'Mot de passe trop petit'.
            // Add a special case for him.

            Debug.LogError(
                $"ERROR: [{GetType().Name}] Request failed, reason: {request.error}"
            );

            Debug.LogError(
                $"ERROR: [{GetType().Name}] Request failed, reason:\n" +
                $"- HTTP Error: {request.error}\n" +
                $"- Response Body:\n{responseBody}"
            );

            yield break;
        }

        Debug.Log(
            $"DEBUG: [{GetType().Name}] Request succeeded, response body:\n{responseBody}"
        );
    }
}