using System.Collections.Generic;
using UnityEngine;

using FrontEnd.Data.Game;

using static DirectionProperties;


public class ResponseSequenceManager : MonoBehaviour
{
    [Header("----- DEBUG -----")]
    [SerializeField] bool _isDebugModeOn;


    bool _isLevelInfinite = false;

    List<Direction> _levelDirections = new();
    int _currentLevelDirectionIndex = -1;


    public void Initialize(Level p_levelProperties)
    {
        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Starting to initialize the {GetType().Name} Class for Level '{p_levelProperties.Name}'.");
        

        _isLevelInfinite = p_levelProperties.IsInfinite;

        _levelDirections = ConvertResponseSequenceToList(p_levelProperties.ResponseSequence);
        _currentLevelDirectionIndex = 0;


        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Successfully initialized the {GetType().Name} Class for Level '{p_levelProperties.Name}'.");
    }


    List<Direction> ConvertResponseSequenceToList(string p_responseSequence)
    {
        List<Direction> directions = new(p_responseSequence.Length);

        for (int i = 0; i < p_responseSequence.Length; i++)
        {
            if (!DIRECTION_CHAR_TO_DIRECTIONS.TryGetValue(p_responseSequence[i], out Direction direction))
            {
                Debug.LogWarning($"WARNING: [{GetType().Name}] The character number {i} of the given string '{p_responseSequence}', is not valid. Character: '{p_responseSequence[i]}'. Continuing.");
                continue;
            }

            directions.Add(direction);
        }

        return directions;
    }


    public bool TryGetCurrentDirection(out Direction p_direction)
    {
        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Trying to get the current direction based on the data received when initializing.");


        if (_currentLevelDirectionIndex < 0)
        {
            Debug.LogWarning($"WARNING: [{GetType().Name}] The '{nameof(_currentLevelDirectionIndex)}' property is inferior to 0. Have you initialized the class? Returning false and {Direction.Left}.");
            p_direction = Direction.Left;

            return false;
        }

        if (_currentLevelDirectionIndex >= _levelDirections.Count)
        {
            Debug.LogWarning($"WARNING: [{GetType().Name}] The '{nameof(_currentLevelDirectionIndex)}' property is superior or equal to {_levelDirections.Count}. Returning false and {Direction.Left}.");
            p_direction = Direction.Left;

            return false;
        }

        p_direction = _levelDirections[_currentLevelDirectionIndex];

        return true;
    }

    public bool TryAdvanceToNextDirection(out Direction p_direction)
    {
        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Trying to advance to the next direction based on the data received when initializing.");


        // -- Handling infinite Level -- //

        if (_isLevelInfinite)
        {
            p_direction = GetRandomDirection();

            if (_isDebugModeOn)
                Debug.Log($"DEBUG: [{GetType().Name}] Successfully advanced to the next direction based on random. Invoking '{nameof(EventHandler.OnNextLevelDirectionComputedEvent)}' Event.");

            EventHandler.OnNextLevelDirectionComputedEvent?.Invoke(p_direction);

            return true;
        }

        // -- Handling finite Level -- //

        if (!TryGetNextLevelDirection(out p_direction))
        {
            Debug.LogWarning($"WARNING: [{GetType().Name}] Failed to advance to the next direction based on the data received when initializing. Returning false and {Direction.Left}.");
            p_direction = Direction.Left;

            return false;
        }

        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Successfully advanced to the next direction based on the data received when initializing. Invoking '{nameof(EventHandler.OnNextLevelDirectionComputedEvent)}' Event.");

        EventHandler.OnNextLevelDirectionComputedEvent?.Invoke(p_direction);

        return true;
    }


    Direction GetRandomDirection()
    {
        return UnityEngine.Random.Range(0, 2) == 0
            ? Direction.Left
            : Direction.Right;
    }

    bool TryGetNextLevelDirection(out Direction p_direction)
    {
        int nextIndex = _currentLevelDirectionIndex + 1;

        if (nextIndex >= _levelDirections.Count)
        {
            Debug.LogWarning(
                $"WARNING: [{GetType().Name}] Reached the end of the Level's direction, {_currentLevelDirectionIndex} + 1 >= {_levelDirections.Count}.\n" +
                "Failed to get the next direction based on the data received when initializing. Returning false and {Direction.Left}."
            );
            p_direction = Direction.Left;

            return false;
        }

        _currentLevelDirectionIndex = nextIndex;
        p_direction = _levelDirections[_currentLevelDirectionIndex];

        return true;
    }
}