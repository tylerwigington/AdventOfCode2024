namespace AdventOfCode2024.Shared;

public sealed record Coordinate(int Row, int Col)
{
    public static Coordinate operator -(Coordinate a, Coordinate b) => new(a.Row - b.Row, a.Col - b.Col);
    public static Coordinate operator +(Coordinate a, Coordinate b) => new(a.Row + b.Row, a.Col + b.Col);
}

public sealed record Offset(int Row, int Col);
