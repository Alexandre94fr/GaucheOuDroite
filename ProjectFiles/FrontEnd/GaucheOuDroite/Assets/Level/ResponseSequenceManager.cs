using UnityEngine;

using FrontEnd.Data.Game;


public class ResponseSequenceManager : MonoBehaviour
{
    [Header("----- DEBUG -----")]
    [SerializeField] bool _isDebugModeOn;


    int _levelId = -1;

    Level _levelProperties;


    void Start()
    {
        
    }


    public void Initialize(int p_levelId, Level p_levelProperties)
    {
        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Starting to initialize the {GetType().Name} Class for Level {p_levelId}.");


        _levelId = p_levelId;

        _levelProperties = p_levelProperties;
    }
}