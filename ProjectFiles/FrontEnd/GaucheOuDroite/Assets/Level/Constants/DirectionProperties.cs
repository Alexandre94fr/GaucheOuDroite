using System.Collections.Generic;


public class DirectionProperties
{
    public enum Direction
    {
        Left,
        Right,
    }

    public static readonly Dictionary<char, Direction> DIRECTION_CHAR_TO_DIRECTIONS = new()
    {
        ['L'] = Direction.Left,
        ['R'] = Direction.Right,
    };

    // -- Localization -- //

    public static readonly Dictionary<Direction, string> DIRECTIONS_IN_FRENCH = new()
    {
        [Direction.Left]    = "Gauche",
        [Direction.Right]   = "Droite",
    };

    public static readonly Dictionary<Direction, string> DIRECTIONS_IN_ENGLISH = new()
    {
        [Direction.Left]    = "Left",
        [Direction.Right]   = "Right",
    };

    public static readonly Dictionary<Direction, string> DIRECTIONS_IN_ENGLISH = new()
    {
        [Direction.Left] = "Left",
        [Direction.Right] = "Right",
    };
}