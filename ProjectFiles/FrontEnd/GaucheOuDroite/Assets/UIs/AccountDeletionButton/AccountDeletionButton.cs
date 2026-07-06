using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using VariableCheckerPackage;

using FrontEnd.Data.Game;
using FrontEnd.Data.User;

using Shared.DTOs;


public class AccountDeletionButton : MonoBehaviour
{
    [Header("Internal references:")]
    [SerializeField] SceneChanger _sceneChanger;

    [Space]
    [SerializeField] Button _confirmationMenuOpenerButton;

    [Space]
    [SerializeField] GameObject _confirmationMenu;

    [Header("Properties:")]
    [SerializeField] string _signUpSceneName = "SignUp";


    bool _isConfirmationMenuOn = false;


    void Start()
    {
        // -- Class properties verifications -- //

        if (!VariablesChecker.AreVariablesValid(name, null,
            (_sceneChanger, nameof(_sceneChanger)),

            (_confirmationMenuOpenerButton, nameof(_confirmationMenuOpenerButton)),

            (_confirmationMenu, nameof(_confirmationMenu))
        )) return;

        if (string.IsNullOrEmpty(_signUpSceneName))
        {
            Debug.LogError(
                $"{VariablesChecker.GetCheckErrorMessagePrefix(name, nameof(_signUpSceneName))} is null or empty, " +
                $"please set it through the Unity inspector. Returning."
            );
            return;
        }
    }


    public void OnButtonPressed()
    {
        if (_isConfirmationMenuOn)
            return;

        _isConfirmationMenuOn = true;

        _confirmationMenuOpenerButton.interactable = false;
        _confirmationMenu.SetActive(true);
    }


    public void OnAccountDeletionAnnulation()
    {
        _isConfirmationMenuOn = false;

        _confirmationMenuOpenerButton.interactable = true;
        _confirmationMenu.SetActive(false);
    }

    IEnumerator DeleteAccount(Button p_deleteAccountButton)
    {
        p_deleteAccountButton.interactable = false;

        // -- Sending a request to the server to delete all the User's data -- //

        yield return ServerRequestManager.SendRequest<ApiResponseDTO>(
            UserDataManager.USER_API_ROUTE,
            ServerRequestManager.RequestType.Delete,

            null,
            true,

            OnRequestSuccess,

            // We don't use a OnRequestFailure method, because we need to access the 'p_deleteAccountButton' Button
            (p_unityWebRequest) =>
            {
                p_deleteAccountButton.interactable = true;
            }
        );
    }

    public void OnAccountDeletionConfirmation(Button p_deleteAccountButton)
    {
        StartCoroutine(DeleteAccount(p_deleteAccountButton));
    }


    void OnRequestSuccess(ApiResponseDTO p_apiResponseDTO)
    {
        // -- Clearing all Game's and User's cached data -- //

        GameDataManager.Instance.ClearAllLocalData();
        UserDataManager.Instance.ClearAllLocalData();

        // -- Clearing AuthenticationToken -- //

        ServerRequestManager.AuthenticationToken = null;

        // -- Switching Scene -- //

        _sceneChanger.LoadAsync(_signUpSceneName);
    }
}