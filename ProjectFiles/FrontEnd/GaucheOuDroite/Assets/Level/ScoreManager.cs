using UnityEngine;

using FrontEnd.Data.Game;
using FrontEnd.Data.User;


public class ScoreManager : MonoBehaviour
{
    [Header("----- DEBUG -----")]
    [SerializeField] bool _isDebugModeOn;


    Level _levelProperties;
    LevelProgression _levelProgression;


    void Start()
    {

    }


    public void Initialize(Level p_levelProperties, LevelProgression p_levelProgression)
    {
        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Starting to initialize the {GetType().Name} Class for Level '{p_levelProperties.Name}'.");


        _levelProperties = p_levelProperties;
        _levelProgression = p_levelProgression;
    }
}