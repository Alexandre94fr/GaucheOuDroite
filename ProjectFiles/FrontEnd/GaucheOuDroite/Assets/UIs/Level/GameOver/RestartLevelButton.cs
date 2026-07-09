using UnityEngine;


public class RestartLevelButton : MonoBehaviour
{
    public void OnButtonPressed()
    {
        LevelManager.Instance.RestartLevel();
    }
}