using UnityEngine;

using FrontEnd.Data.Game;
using FrontEnd.Data.User;


public class QuitGameButton : MonoBehaviour
{
    public void OnButtonPressed()
    {
        // -- Clearing all Game's and User's cached data -- //

        GameDataManager.Instance.ClearAllLocalData();
        UserDataManager.Instance.ClearAllLocalData();

        // -- Clearing AuthenticationToken -- //

        ServerRequestManager.AuthenticationToken = null;

        // -- Quitting the game -- //

        Application.Quit();
    }
}