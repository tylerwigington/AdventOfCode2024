using System.Diagnostics;
using System.Text.RegularExpressions;
using AdventOfCode2024.Shared;

namespace AdventOfCode2024.Problems;

internal class Antenna
{
    public required string Frequency { get; init; }
    public required int Row { get; init; }
    public int ColumnStart { get; init; }

    public int Length => Frequency.Length;
    public (int ColStart, int ColEnd) ColumnPosition => (ColumnStart, ColumnStart + Length - 1);
    private Coordinate? _coordinate;
    public Coordinate ToCoordinate()
    {
        _coordinate ??= new(Row, ColumnStart);
        return _coordinate;
    }
    public override string ToString()
    {
        return string.Format("({0},{1})", Row, ColumnStart);
    }
}

public static partial class Day8
{
    private static readonly Regex _frequency = FreguencyRegex();

    public static async Task<int> Part1Async()
    {
        using var file = File.OpenText("./Inputs/day8.txt");
        Dictionary<string, List<Coordinate>> antennas = [];

        var numRows = 0;
        var numCols = 0;
        while (!file.EndOfStream)
        {
            var line = await file.ReadLineAsync();
            if (string.IsNullOrEmpty(line)) continue;
            numCols = Math.Max(numCols, line.Length);
            var matches = _frequency.Matches(line);
            foreach(Match match in matches)
            {
                if(!antennas.ContainsKey(match.Value))
                    antennas.Add(match.Value, []);
                antennas[match.Value].Add(new(numRows, match.Index));
            }
            numRows++;
        }

        var antiNodes = new HashSet<Coordinate>();
        foreach (var group in antennas)
        {
            for (int i = 0; i < group.Value.Count; i++)
            {
                for (int j = 0; j < group.Value.Count; j++)
                {
                    var first = group.Value[i];
                    var second = group.Value[j];
                    if(first == second) continue;
                    if (TryGetAntinode(first, second, numRows, numCols, out var result))
                        antiNodes.Add(result!);
                    if (TryGetAntinode(second, first, numRows, numCols, out var result2))
                        antiNodes.Add(result2!);

                }
            }
        }
        return antiNodes.Count;
    }

    public static async Task<int> Part2Async()
    {
        using var file = File.OpenText("./Inputs/day8.txt");
        Dictionary<string, List<Coordinate>> antennas = [];

        var rowBound = 0;
        var columnBound = 0;
        while (!file.EndOfStream)
        {
            var line = await file.ReadLineAsync();
            if (string.IsNullOrEmpty(line)) continue;
            columnBound = line.Length;
            var matches = _frequency.Matches(line);
            foreach (Match match in matches)
            {
                if (!antennas.ContainsKey(match.Value))
                    antennas.Add(match.Value, []);
                antennas[match.Value].Add(new(rowBound, match.Index));
            }
            rowBound++;
        }

        var antiNodes = new List<Coordinate>();
        foreach (var group in antennas)
        {
            for (int i = 0; i < group.Value.Count; i++)
            {
                for (int j = 0; j < group.Value.Count; j++)
                {
                    var first = group.Value[i];
                    var second = group.Value[j];
                    if (first == second) continue;
                    if (TryGetAllAntinodes(first, second, rowBound, columnBound, out var result))
                        antiNodes.AddRange(result!);
                    if (TryGetAllAntinodes(second, first, rowBound, columnBound, out var result2))
                        antiNodes.AddRange(result2!);

                }
            }
        }
        return antiNodes.Distinct().ToList().Count;
    }

    private static bool TryGetAntinode(Coordinate a, Coordinate b, int rowMax, int colMax, out Coordinate? result)
    {
        var row = b.Row + (b.Row - a.Row);
        var col = b.Col + (b.Col - a.Col);
        if (row >= 0 && row < rowMax && col >= 0 && col < colMax)
        {
            result = new(row, col);
            return true;
        }
        result = null;
        return false;
    }

    private static bool TryGetAllAntinodes(Coordinate a, Coordinate b, int rowMax, int colMax, out List<Coordinate>? result)
    {
        result = [];
        var row = b.Row + (b.Row - a.Row);
        var col = b.Col + (b.Col - a.Col);
        result.Add(new(b.Row, b.Col));
        while (row >= 0 && row < rowMax && col >= 0 && col < colMax)
        {
            result.Add(new(row, col));
            row += (b.Row - a.Row);
            col += (b.Col - a.Col);
        }
        return result.Count > 0;
    }

    [GeneratedRegex("[0-9A-Za-z]")]
    private static partial Regex FreguencyRegex();
}

public sealed record Coordinate(int Row, int Col);
