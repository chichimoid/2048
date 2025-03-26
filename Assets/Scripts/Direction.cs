using UnityEngine;

public enum Direction
{
    Up,
    Down,
    Right,
    Left
}

public static class DirectionExtensions
{
    public static Vector2Int ToVector2Int(this Direction direction)
    {
        return direction switch
        {
            Direction.Up => Vector2Int.up,
            Direction.Down => Vector2Int.down,
            Direction.Right => Vector2Int.right,
            Direction.Left => Vector2Int.left,
            _ => Vector2Int.zero
        };
    }
}