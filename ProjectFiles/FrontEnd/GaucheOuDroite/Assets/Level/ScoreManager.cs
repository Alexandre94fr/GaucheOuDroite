using UnityEngine;

using FrontEnd.Data.Game;
using FrontEnd.Data.User;


/// <summary>
/// The <see cref="ScoreManager"/> will not implement any methods in order to save a new best score. <para></para>
/// 
/// The <see cref="LevelManager"/> should handle that.
/// </summary>
public class ScoreManager : MonoBehaviour
{
    [Header("----- DEBUG -----")]
    [SerializeField] bool _isDebugModeOn;


    Level _levelProperties;
    LevelProgression _levelProgression;

    int _score = 0;


    public void Initialize(Level p_levelProperties, LevelProgression p_levelProgression)
    {
        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Starting to initialize the {GetType().Name} Class for Level '{p_levelProperties.Name}'.");

        _levelProperties = p_levelProperties;
        _levelProgression = p_levelProgression;

        SetScore(0);

        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Successfully initialized the {GetType().Name} Class for Level '{p_levelProperties.Name}'.");
    }


    public int GetScore()
    {
        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Getting the score.");

        return _score;
    }

    void SetScore(int p_newValue)
    {
        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Starting to set the score from {_score} to {p_newValue}.");

        // -- Updating the ScoreManager -- //

        _score = p_newValue;

        // -- Invoking event -- //

        EventHandler.OnScoreChangedEvent?.Invoke(_score);

        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Successfully set the score to {_score} and invoked the {nameof(EventHandler.OnScoreChangedEvent)} Event.");
    }

    public void AddScore(int p_addedValue)
    {
        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Starting to add {p_addedValue} to the score.");

        // -- Security -- //

        if (p_addedValue < 0)
        {
            Debug.LogWarning($"WARNING: [{GetType().Name}] Tried to add a negative value to the score. Returning.");
            return;
        }

        // -- Updating the ScoreManager -- //

        SetScore(GetScore() + p_addedValue);

        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Successfully added {p_addedValue} to the score and invoked the {nameof(EventHandler.OnScoreChangedEvent)} Event.");
    }


    public int ConvertScoreToStars()
    {
        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Starting to convert the score into stars.");

        int starsNumber = 0;

        if (_score >= _levelProperties.Star1MinimumScore)
            starsNumber = 1;

        if (_score >= _levelProperties.Star2MinimumScore)
            starsNumber = 2;

        if (_score >= _levelProperties.Star3MinimumScore)
            starsNumber = 3;

        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Successfully converted the score into stars, {_score} score -> {starsNumber} stars.");

        return starsNumber;
    }


    public bool HasNewBestScore()
    {
        return _score > _levelProgression.BestScore;
    }
}