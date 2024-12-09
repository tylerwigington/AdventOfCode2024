namespace AdventOfCode2024.Problems;

public record Direction(int Row, int Col);

public record Turn(int Row, int Col);

public static class Day6
{
    private const string _guard = "^";
    private const string _block = "#";
    private const string _visited = "X";
    private const string _turn = "+";
    private const string _clearPath = ".";
    private static readonly Direction _up = new(-1, 0);
    private static readonly Direction _down = new(1, 0);
    private static readonly Direction _left = new(0, -1);
    private static readonly Direction _right = new(0, 1);

    private static readonly Direction[] _directions =
    [
        _up,
        _right,
        _down,
        _left
    ];
    
    public static async Task<int> Part1Async()
    {
        using var file = File.OpenText("./Inputs/day6_test.txt");
        var map = (await file.ReadToEndAsync())
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(row => row.ToList().Select(c => c.ToString()).ToList())
            .ToList();

        (int Row, int Col) start = (-1,-1);
        for (var iRow = 0; iRow < map.Count; iRow++)
        {
            for (var jCol = 0; jCol < map[iRow].Count; jCol++)
            {
                if (map[iRow][jCol] != _guard) continue;
                start = (iRow, jCol);
                break;
            }
        }

        var directionIndex = 0;
        // var currentDirection = MoveDirection.Up;
        var row = start.Row;
        var col = start.Col;
        var uniquePoints = new HashSet<string>();
        // var turns = new List<Turn> { new(row, col, MoveDirection.Left) };
        while (true)
        {
            var direction = _directions[directionIndex % 4];
            if (!InBounds(map, row, col, direction))
            {
                // turns.Add(new Turn(col, row, currentDirection));
                break;
            };

            if (map[row + direction.Row][col + direction.Col] == _block)
            {
                // map[row][col] = _turn;
                // var prev = currentDirection;
                directionIndex++;
                // currentDirection = ChangeDirection(currentDirection);
                // turns.Add(new Turn(row, col, prev));
                continue;
            };
            
            map[row + direction.Row][col + direction.Col] = _guard;
            uniquePoints.Add($"{row+direction.Row},{col+direction.Col}");
            map[row][col] = _clearPath;
            row += direction.Row;
            col += direction.Col;
        }
        PrintMap(map);
        return uniquePoints.Count;
    }
    
    private static bool InBounds(List<List<string>> map, int row, int col, Direction direction)
    {
        return row + direction.Row >= 0 &&
               row + direction.Row < map[0].Count &&
               col + direction.Col >= 0 &&
               col + direction.Col < map.Count;
    }
    
    private static void PrintMap(List<List<string>> map)
    {
        foreach (var row in map)
        {
            foreach(var cell in row)
            {
                Console.Write(cell);
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}