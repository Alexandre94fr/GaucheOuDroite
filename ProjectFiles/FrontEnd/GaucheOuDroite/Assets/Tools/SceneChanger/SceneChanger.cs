using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [Header("----- DEBUG -----")]
    public bool IsDebugModeOn;

    public void SwitchTo(string p_newScene)
    {
        if (IsDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Trying to load the '{p_newScene}' Scene.");

        SceneManager.LoadScene(p_newScene);
    }

    public void SwitchToAsync(string p_newScene)
    {
        if (IsDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Trying to load asynchronously the '{p_newScene}' Scene.");

        SceneManager.LoadSceneAsync(p_newScene);
    }
}