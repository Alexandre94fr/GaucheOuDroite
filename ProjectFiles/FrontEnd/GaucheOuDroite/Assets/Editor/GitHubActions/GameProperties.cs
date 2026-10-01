using UnityEngine;

using FrontEnd.Data.Game;


public static class GameProperties
{
    public static string GetGameVersion()
    {
        return GameDataManager.GAME_VERSION;
    }

    /// <summary>
    /// Prints the game version in this form: <c> "GAME_VERSION=x.y.z" </c> -> <c> "GAME_VERSION=1.1.2" </c> or <c> "GAME_VERSION=4.12.36" </c>.
    /// 
    /// <para> We put <c> "GAME_VERSION=" </c> before the version so it's easier to find this special log between all the other Unity logs. </para>
    /// </summary>
    public static void PrintGameVersion()
    {
        Debug.Log($"GAME_VERSION={GetGameVersion()}");
    }
}