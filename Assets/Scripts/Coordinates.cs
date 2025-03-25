using System;

public struct Coordinates : IEquatable<Coordinates>
{
    public Coordinates(int x, int y)
    {
        X = x;
        Y = y;
    }
    public int X {get; set;}
    public int Y {get; set;}

    
    /// <summary>
    /// Hardcoded implementation for quicker equality checks.
    /// </summary>
    public bool Equals(Coordinates other)
    {
        return X == other.X && Y == other.Y;
    }

    /// <summary>
    /// Hardcoded implementation for quicker equality checks.
    /// </summary>
    public override bool Equals(object obj)
    {
        return obj is Coordinates other && Equals(other);
    }

    /// <summary>
    /// Hardcoded implementation for quicker equality checks.
    /// </summary>
    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}