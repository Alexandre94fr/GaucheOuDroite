using UnityEngine;

using FrontEnd.Data.Game;
using FrontEnd.Data.User;


public class ScoreManager : MonoBehaviour
{
    [Header("----- DEBUG -----")]
    [SerializeField] bool _isDebugModeOn;


    int _levelId = -1;

    Level _levelProperties;
    LevelProgression _levelProgression;


    void Start()
    {

    }


    public void Initialize(int p_levelId, Level p_levelProperties, LevelProgression p_levelProgression)
    {
        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Starting to initialize the {GetType().Name} Class for Level {p_levelId}.");


        _levelId = p_levelId;

        _levelProperties = p_levelProperties;
        _levelProgression = p_levelProgression;
    }
}