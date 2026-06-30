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
}