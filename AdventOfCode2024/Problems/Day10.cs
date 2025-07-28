using System.Diagnostics;

namespace AdventOfCode2024.Problems;

public static class Day10
{
    private static readonly (int Row, int Col)[] _directions =
    [
        (-1,0),
        (0,1),
        (1,0),
        (0,-1)
    ];

    private const int _trailHeadKey = 0;
    private const int _trailTailKey = 9;

    private static readonly Stopwatch stopwatch = new();

    public static async Task<long> Part1Async()
    {
        using var file = File.OpenText("./Inputs/day10_test.txt");
        var graph = new List<List<int>>();
        var topo = new Dictionary<int, List<Shared.Coordinate>>();
        var rows = 0;
        var cols = 0;
        while (!file.EndOfStream)
        {
            var line = await file.ReadLineAsync();
            if (string.IsNullOrEmpty(line)) continue;
            graph.Add(line.Select(c => (int)char.GetNumericValue(c)).ToList());
            for (var col = 0; col < line.Length; col++)
            {
                var key = int.Parse(line[col].ToString());
                if (!topo.ContainsKey(key))
                    topo.Add(key, []);
                topo[key].Add(new Shared.Coordinate(rows, col));
            }
            cols = Math.Max(line.Length, cols);
            rows++;
        }
        var sum = 0;
        foreach (var trailhead in topo[_trailHeadKey])
        {
            sum += FindTrail(topo, trailhead, 1)?.Count ?? 0;
        }
        return sum;
    }

    private static List<Shared.Coordinate> FindTrail(Dictionary<int, List<Shared.Coordinate>> topo, Shared.Coordinate head, int depth)
    {
        if (depth == _trailTailKey)
        {
            return topo[_trailTailKey].Where(coord => coord.IsAdjacent(head))?.ToList() ?? [];
        }

        List<Shared.Coordinate> coords = [];
        foreach (var coord in topo[depth])
        {
            if (coord.IsAdjacent(head))
            {
                var res = FindTrail(topo, coord, depth + 1);
                coords.AddRange(res);
            }
        }
        return coords;

    }
}