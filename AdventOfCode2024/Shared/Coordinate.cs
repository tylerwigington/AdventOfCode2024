namespace AdventOfCode2024.Shared;

public static class Directions
{
    public const int Up = -1;
    public const int Down = 1;
    public const int Left = -1;
    public const int Right = 1;
    public static readonly List<Coordinate> AdjacentNoDiagonal =
    [
        new(-1,0),
        new(1,0),
        new(0,-1),
        new(0,1)
    ];
}

public sealed class Coordinate(int row, int col) : IEquatable<Coordinate>
{
    public static readonly List<Coordinate> AdjacentNoDiagonal =
    [
        new(-1,0),
        new(0,-1),
        new(1,0),
        new(0,1)
    ];

    public int Row { get; init; } = row;
    public int Col { get; init; } = col;

    public bool IsAdjacent(Coordinate other)
    {
        return AdjacentNoDiagonal.Select(p => p + this).Any(p => p == other);
    }

    public bool Equals(Coordinate? other)
    {
        if (other is null) return false;
        return other == this;
    }

    public static Coordinate operator -(Coordinate a, Coordinate b) => new(a.Row - b.Row, a.Col - b.Col);
    public static Coordinate operator +(Coordinate a, Coordinate b) => new(a.Row + b.Row, a.Col + b.Col);
    public static bool operator ==(Coordinate a, Coordinate b) => a.Row == b.Row && a.Col == b.Col;
    public static bool operator !=(Coordinate a, Coordinate b) => a.Row != b.Row && a.Col != b.Col;

    public override bool Equals(object? obj)
    {
        return Equals(obj as Coordinate);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Row, Col);
    }

    public bool InBounds(int height, int width) => Row >= 0 && Row < height && Col >= 0 && Col < width;
}
