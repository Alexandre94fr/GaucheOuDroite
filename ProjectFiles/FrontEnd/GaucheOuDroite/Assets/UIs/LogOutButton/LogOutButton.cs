using UnityEngine;

using VariableCheckerPackage;

using FrontEnd.Data.Game;
using FrontEnd.Data.User;


public class LogOutButton : MonoBehaviour
{
    [Header("Internal references:")]
    [SerializeField] SceneChanger _sceneChanger;

    [Header("Properties:")]
    [SerializeField] string _logInSceneName = "LogIn";


    void Start()
    {
        // -- Class properties verifications -- //

        if (!VariablesChecker.AreVariablesValid(name, null,
            (_sceneChanger, nameof(_sceneChanger))
        )) return;

        if (string.IsNullOrEmpty(_logInSceneName))
        {
            Debug.LogError(
                $"{VariablesChecker.GetCheckErrorMessagePrefix(name, nameof(_logInSceneName))} is null or empty, " +
                $"please set it through the Unity inspector. Returning."
            );
            return;
        }
    }


    public void OnButtonPressed()
    {
        // -- Clearing all Game's and User's cached data -- //

        GameDataManager.Instance.ClearAllLocalData();
        UserDataManager.Instance.ClearAllLocalData();

        // -- Clearing AuthenticationToken -- //

        ServerRequestManager.AuthenticationToken = null;

        // -- Switching Scene -- //

        _sceneChanger.SwitchToAsync(_logInSceneName);
    }
}