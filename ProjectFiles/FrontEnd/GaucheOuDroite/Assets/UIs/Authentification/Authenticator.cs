using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using VariableCheckerPackage;

using FrontEnd.Data.Game;
using FrontEnd.Data.User;

using Shared.Constants;
using Shared.Tools;

// For networking

using UnityEngine.Networking;

using Newtonsoft.Json;

using Shared.DTOs;


public class Authenticator : MonoBehaviour
{
    [Header("----- DEBUG -----")]
    [SerializeField] bool _isDebugModeOn;


    [Header("Internal references:")]
    [SerializeField] Button _requestSenderButton;
    [SerializeField] Button _autenticationModeChangerButton;

    [Space]
    [SerializeField] TextMeshProUGUI _feedbackText;

    [Space]
    [SerializeField] GameObject _autenticationInformationGameObject;

    [Space]
    [SerializeField] SceneChanger _sceneChanger;

    [Header("Properties:")]
    [SerializeField] AuthenticationProperties.AuthenticationMode _authenticationMode = AuthenticationProperties.AuthenticationMode.SignUp;

    [Space]
    [SerializeField] string _levelSelectionSceneName = "LevelSelection";


    string _username = "";
    string _password = "";


    void Start()
    {
        // -- Class properties verifications -- //

        if (!VariablesChecker.AreVariablesValid(name, null,
            (_requestSenderButton, nameof(_requestSenderButton)),
            (_autenticationModeChangerButton, nameof(_autenticationModeChangerButton)),
            (_feedbackText, nameof(_feedbackText)),
            (_autenticationInformationGameObject, nameof(_autenticationInformationGameObject)),
            (_sceneChanger, nameof(_sceneChanger))
        )) return;

        if (string.IsNullOrEmpty(_levelSelectionSceneName))
        {
            Debug.LogWarning($"DEBUG: [{GetType().Name}] The '{nameof(_levelSelectionSceneName)}' property is null or empty. Returning.");
            return;
        }
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
        return UserDataChecker.IsUsernameValid(p_username, out p_authenticationErrorReason, _isDebugModeOn);
    }

    bool IsPasswordValid(string p_password, out AuthenticationProperties.AuthenticationErrorReasons p_authenticationErrorReason)
    {
        return UserDataChecker.IsPasswordValid(p_password, out p_authenticationErrorReason, _isDebugModeOn);
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

            _autenticationInformationGameObject.SetActive(true);

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

            _autenticationInformationGameObject.SetActive(true);

            if (_isDebugModeOn)
                Debug.Log($"DEBUG: [{GetType().Name}] The given password '{new string('*', _password.Length)}' is not valid, reason: {errorMessage} Returning");

            return;
        }

        #endregion

        // Modifying the UI to tell the player, that his username and password are correct and that we sent a request to the server.
        string authenticationModeString = AuthenticationProperties.AUTHENTICATION_MODE_IN_FRENCH[_authenticationMode].ToUpper();

        DisplayFeedback(
            AuthenticationProperties.SUCCESSFUL_LOCAL_AUTHENTICATION_MESSAGE,
            new(
                AuthenticationProperties.AUTHENTICATION_SUCCESS_MESSAGE_COLOR.X,
                AuthenticationProperties.AUTHENTICATION_SUCCESS_MESSAGE_COLOR.Y,
                AuthenticationProperties.AUTHENTICATION_SUCCESS_MESSAGE_COLOR.Z
            )
        );

        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] The given username and password are correct, sending them to the Authentication API.");

        // Sending these values to the correct Authentication API (in BackEnd)
        // Will also disable some UI buttons to avoid creating bugs.
        // The UI buttons will be interactive again when the server will respond to the request.
        StartCoroutine(SendAuthenticationRequestToServer(_authenticationMode));
    }

    AuthenticationResultDTO ConvertRequestResponseData(UnityWebRequest p_requestResponse)
    {
        return JsonConvert.DeserializeObject<AuthenticationResultDTO>(p_requestResponse.downloadHandler.text);
    }

    IEnumerator SendAuthenticationRequestToServer(AuthenticationProperties.AuthenticationMode p_authenticationMode)
    {
        // Disabling some UI buttons to avoid creating bugs.
        // The UI buttons will be interactive again when the server will respond to the request.
        _requestSenderButton.interactable = false;
        _autenticationModeChangerButton.interactable = false;

        // Getting and creating the good route and DTO depending of the given AuthenticationMode 
        string route;
        object authenticationDTO;

        switch (p_authenticationMode)
        {
            case AuthenticationProperties.AuthenticationMode.SignUp:

                route = "authentication/sign-up";

                authenticationDTO = new SignUpDTO()
                {
                    Username = _username,
                    Password = _password
                };

                break;

            case AuthenticationProperties.AuthenticationMode.LogIn:

                route = "authentication/log-in";

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

        yield return ServerRequestManager.SendRequest<AuthenticationResultDTO>(
            route,
            ServerRequestManager.RequestType.Post,

            authenticationDTO,
            false,

            OnRequestSuccess,
            OnRequestFailure
        );

        // We received a response from the server, we can make the buttons intractable again
        _requestSenderButton.interactable = true;
        _autenticationModeChangerButton.interactable = true;
    }

    void OnRequestSuccess(AuthenticationResultDTO p_authenticationResultDTO)
    {
        DisplayFeedback(
            AuthenticationProperties.SUCCESSFUL_SERVER_AUTHENTICATION_MESSAGE,
            new(
                AuthenticationProperties.AUTHENTICATION_SUCCESS_MESSAGE_COLOR.X,
                AuthenticationProperties.AUTHENTICATION_SUCCESS_MESSAGE_COLOR.Y,
                AuthenticationProperties.AUTHENTICATION_SUCCESS_MESSAGE_COLOR.Z
            )
        );

        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Request succeeded, authentication succeeded.");

        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Saving the AuthenticationToken inside the ServerRequestManager Class.");

        ServerRequestManager.AuthenticationToken = p_authenticationResultDTO.Token;

        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Loading the Game's and User's data, and switching Scene to '{_levelSelectionSceneName}' when the loading ends");

        StartCoroutine(LoadAllDataAndChangeScene());
    }

    IEnumerator LoadAllDataAndChangeScene()
    {
        bool hasLoadingTookTooLong = false;

        // In this case we will not use the callbacks,
        // only the HasLoadedServerData properties.
        // It's more compact to do so.

        StartCoroutine(GameDataManager.Instance.LoadDataFromServerAsync());
        StartCoroutine(UserDataManager.Instance.LoadDataFromServerAsync());

        yield return new WaitUntil(
            () =>
                GameDataManager.Instance.HasLoadedServerData &&
                UserDataManager.Instance.HasLoadedServerData,

            AuthenticationProperties.MAXIMAL_INITIAL_DATA_LOADING_TIME,

            () =>
            {
                hasLoadingTookTooLong = true;

                DisplayFeedback(
                    AuthenticationProperties.TOO_LONG_LOADING_DATA_ERROR_MESSAGE,
                    new(
                        AuthenticationProperties.AUTHENTICATION_ERROR_MESSAGE_COLOR.X,
                        AuthenticationProperties.AUTHENTICATION_ERROR_MESSAGE_COLOR.Y,
                        AuthenticationProperties.AUTHENTICATION_ERROR_MESSAGE_COLOR.Z
                    )
                );

                Debug.LogError($"ERROR: [{GetType().Name}] The loading of all the data took too long ({AuthenticationProperties.MAXIMAL_INITIAL_DATA_LOADING_TIME.TotalSeconds}s). Stopping the coroutine. Returning.");
            }
        );

        if (hasLoadingTookTooLong)
            yield break;

        _sceneChanger.SwitchToAsync(_levelSelectionSceneName);
    }

    void OnRequestFailure(UnityWebRequest p_request)
    {
        AuthenticationResultDTO authenticationResultDTO = ConvertRequestResponseData(p_request);

        // Handling all cases
        switch (p_request.result)
        {
            case UnityWebRequest.Result.Success:

                // The case is already handled by the OnRequestSuccess method
                break;

            case UnityWebRequest.Result.ConnectionError:
                {
                    DisplayFeedback(
                        AuthenticationProperties.SERVER_CONNECTION_ERROR_MESSAGE,
                        new(
                            AuthenticationProperties.AUTHENTICATION_ERROR_MESSAGE_COLOR.X,
                            AuthenticationProperties.AUTHENTICATION_ERROR_MESSAGE_COLOR.Y,
                            AuthenticationProperties.AUTHENTICATION_ERROR_MESSAGE_COLOR.Z
                        )
                    );

                    Debug.LogError($"ERROR: [{GetType().Name}] Request failed ({p_request.result}). {ServerRequestManager.RequestResponseToString(p_request)}\nReturning.");
                    break;
                }

            case UnityWebRequest.Result.ProtocolError:

                // Using the .AuthenticationError value to get a string from the AuthenticationProperties.AUTHENTICATION_ERROR_MESSAGES dictionary
                // If we fail, the shown error will be an Unknown error message
                if (!AuthenticationProperties.AUTHENTICATION_ERROR_MESSAGES.TryGetValue(authenticationResultDTO.AuthenticationError, out string authenticationMessage))
                {
                    authenticationMessage = AuthenticationProperties.UNKNOWN_ERROR_MESSAGE;
                }

                DisplayFeedback(
                    authenticationMessage,
                    new(
                        AuthenticationProperties.AUTHENTICATION_ERROR_MESSAGE_COLOR.X,
                        AuthenticationProperties.AUTHENTICATION_ERROR_MESSAGE_COLOR.Y,
                        AuthenticationProperties.AUTHENTICATION_ERROR_MESSAGE_COLOR.Z
                    )
                );

                Debug.LogWarning($"WARNING: [{GetType().Name}] Request failed ({p_request.result}). {ServerRequestManager.RequestResponseToString(p_request)}\nReturning.");
                break;

            case UnityWebRequest.Result.DataProcessingError:

                DisplayFeedback(
                    AuthenticationProperties.DATA_PROCESSING_ERROR_MESSAGE,
                    new(
                        AuthenticationProperties.AUTHENTICATION_ERROR_MESSAGE_COLOR.X,
                        AuthenticationProperties.AUTHENTICATION_ERROR_MESSAGE_COLOR.Y,
                        AuthenticationProperties.AUTHENTICATION_ERROR_MESSAGE_COLOR.Z
                    )
                );

                Debug.LogError($"ERROR: [{GetType().Name}] Request failed ({p_request.result}). {ServerRequestManager.RequestResponseToString(p_request)}\nReturning.");
                break;

            default:

                Debug.LogWarning($"WARNING: [{GetType().Name}] The received '{p_request.result}' UnityWebRequest.Result is not planned in the switch. Showing unknown error and returning.");

                DisplayFeedback(
                    AuthenticationProperties.UNKNOWN_ERROR_MESSAGE,
                    new(
                        AuthenticationProperties.AUTHENTICATION_ERROR_MESSAGE_COLOR.X,
                        AuthenticationProperties.AUTHENTICATION_ERROR_MESSAGE_COLOR.Y,
                        AuthenticationProperties.AUTHENTICATION_ERROR_MESSAGE_COLOR.Z
                    )
                );

                Debug.LogError($"ERROR: [{GetType().Name}] Request failed ({p_request.result}). {ServerRequestManager.RequestResponseToString(p_request)}\nReturning.");
                break;
        }
    }
}