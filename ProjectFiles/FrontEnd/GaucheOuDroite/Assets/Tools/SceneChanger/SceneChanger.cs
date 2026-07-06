using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneChanger : MonoBehaviour
{
    [Header("----- DEBUG -----")]
    public bool IsDebugModeOn = false;


    public Action<Scene, LoadSceneMode> OnSceneLoadedEvent;
    public Action<Scene, LoadSceneMode> OnSceneLoadedAfterStartCallEvent;


    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    void OnSceneLoaded(Scene p_scene, LoadSceneMode p_loadSceneMode)
    {
        if (IsDebugModeOn)
        {
            Debug.Log($"DEBUG: [{GetType().Name}] A new Scene as been loaded: (Scene name: {p_scene.name}, Load mode: {p_loadSceneMode}).");
            Debug.Log($"DEBUG: [{GetType().Name}] Trying invoking the '{nameof(OnSceneLoadedEvent)}' Event.");
        }

        OnSceneLoadedEvent?.Invoke(p_scene, p_loadSceneMode);

        // Wait a frame before invoking the OnSceneLoadedAfterStartCallEvent
        StartCoroutine(OnSceneLoadedAfterStartCall(p_scene, p_loadSceneMode));
    }

    IEnumerator OnSceneLoadedAfterStartCall(Scene p_scene, LoadSceneMode p_loadSceneMode)
    {
        // Waiting one frame
        yield return null;

        if (IsDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Trying invoking the '{nameof(OnSceneLoadedAfterStartCallEvent)}' Event.");

        OnSceneLoadedAfterStartCallEvent?.Invoke(p_scene, p_loadSceneMode);
    }


    // Note:
    // We need to have a Load and a LoadAsync method with only one parameter, otherwise, the UnityEvent in UIs can't call the methods.

    /// <summary>
    /// Loads synchronously the given Scene. <para></para>
    /// 
    /// If you want to know when the Scene is loaded, you can use the:
    /// 
    /// <list type="bullet">
    /// <item><description> <see cref="OnSceneLoadedEvent"/> Action</description></item>
    /// <item><description> <see cref="OnSceneLoadedAfterStartCallEvent"/> Action </description></item>
    /// </list>
    /// 
    /// </summary>
    /// <param name="p_newScene"></param>
    /// <param name="p_loadSceneMode"></param>
    public void Load(string p_newScene, LoadSceneMode p_loadSceneMode)
    {
        if (IsDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Trying to load synchronously the '{p_newScene}' Scene. Load mode: {p_loadSceneMode}.");

        SceneManager.LoadScene(p_newScene, p_loadSceneMode);
    }

    /// <summary>
    /// Loads asynchronously the given Scene. <para></para>
    /// 
    /// If you want to know when the Scene is loaded, you can use the:
    /// 
    /// <list type="bullet">
    /// <item><description> <see cref="OnSceneLoadedEvent"/> Action</description></item>
    /// <item><description> <see cref="OnSceneLoadedAfterStartCallEvent"/> Action </description></item>
    /// </list>
    /// 
    /// </summary>
    /// <param name="p_newScene"></param>
    public void Load(string p_newScene)
    {
        Load(p_newScene, LoadSceneMode.Single);
    }

    /// <summary>
    /// Loads asynchronously the given Scene. <para></para>
    /// 
    /// If you want to know when the Scene is loaded, you can use the:
    /// 
    /// <list type="bullet">
    /// <item><description> <see cref="OnSceneLoadedEvent"/> Action</description></item>
    /// <item><description> <see cref="OnSceneLoadedAfterStartCallEvent"/> Action </description></item>
    /// </list>
    /// 
    /// </summary>
    /// <param name="p_newScene"></param>
    public void LoadAsync(string p_newScene, LoadSceneMode p_loadSceneMode)
    {
        if (IsDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Trying to load asynchronously the '{p_newScene}' Scene. Load mode: {p_loadSceneMode}.");

        SceneManager.LoadSceneAsync(p_newScene, p_loadSceneMode);
    }

    /// <summary>
    /// Loads asynchronously the given Scene. <para></para>
    /// 
    /// If you want to know when the Scene is loaded, you can use the:
    /// 
    /// <list type="bullet">
    /// <item><description> <see cref="OnSceneLoadedEvent"/> Action</description></item>
    /// <item><description> <see cref="OnSceneLoadedAfterStartCallEvent"/> Action </description></item>
    /// </list>
    /// 
    /// </summary>
    /// <param name="p_newScene"></param>
    public void LoadAsync(string p_newScene)
    {
        LoadAsync(p_newScene, LoadSceneMode.Single);
    }
}