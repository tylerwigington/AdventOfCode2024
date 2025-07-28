using System.Text.Json;
using AdventOfCode2024.Shared;

namespace AdventOfCode2024.Problems;

public sealed class Day12 : IPuzzleSolution<long>
{
    private int _height;
    private int _width;
    private readonly List<List<char>> _map = [];
    private readonly HashSet<Coordinate> _visited = [];

    public async Task<long> SolveAsync()
    {
        var file = File.OpenText("./Inputs/day12.txt");
        var rows = 0;
        do
        {
            var line = await file.ReadLineAsync();
            _map.Add([.. line!]);
            rows++;
        } while (!file.EndOfStream);
        _height = rows;
        _width = _map.First()!.Count;

        var total = 0;
        for (int row = 0; row < _height; row++)
        {
            for (int col = 0; col < _width; col++)
            {
                var coord = new Coordinate(row, col);
                if (!_visited.Contains(coord))
                {
                    var (area, perimeter) = FindGroup(coord);
                    total += area * perimeter;
                }
            }
        }
        return total;
    }

    private (int area, int perimeter) FindGroup(Coordinate coordinate)
    {
        var queue = new Queue<Coordinate>();
        var plotPoints = new HashSet<Coordinate>();
        queue.Enqueue(coordinate);
        _visited.Add(coordinate);
        var totalPerimeter = 0;
        while (queue.Count > 0)
        {
            var coord = queue.Dequeue();
            plotPoints.Add(coord);
            var plotPerimeter = 0;
            foreach (var direction in Coordinate.AdjacentNoDiagonal)
            {
                var neighbor = coord + direction;
                if (neighbor.InBounds(_height, _width))
                {
                    var type = _map[neighbor.Row][neighbor.Col];
                    if (type == _map[coord.Row][coord.Col])
                    {
                        if (_visited.Contains(neighbor))
                        {
                            continue;
                        }
                        _visited.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                    else
                    {
                        plotPerimeter++;
                    }
                }
                else
                {
                    plotPerimeter++;
                }
            }
            totalPerimeter += plotPerimeter;
        }
        return (plotPoints.Count, totalPerimeter);
    }
}

public sealed class Day12P2 : IPuzzleSolution<long>
{
    private int _height;
    private int _width;
    private readonly List<List<char>> _map = [];
    private readonly HashSet<Coordinate> _visited = [];
    private readonly List<HashSet<Coordinate>> _plots = [];

    public async Task<long> SolveAsync()
    {
        var file = File.OpenText("./Inputs/day12_test.txt");
        var rows = 0;
        do
        {
            var line = await file.ReadLineAsync();
            _map.Add([.. line!]);
            rows++;
        } while (!file.EndOfStream);
        _height = rows;
        _width = _map.First()!.Count;

        var total = 0;
        for (int row = 0; row < _height; row++)
        {
            for (int col = 0; col < _width; col++)
            {
                var coord = new Coordinate(row, col);
                if (!_visited.Contains(coord))
                {
                    var (area, numSides) = FindGroup(coord);
                    Console.WriteLine(numSides);
                    total += area * numSides;
                }
            }
        }
        return total;
    }

    private (int area, int numSides) FindGroup(Coordinate coordinate)
    {
        var queue = new Queue<Coordinate>();
        var plotPoints = new HashSet<Coordinate>();
        queue.Enqueue(coordinate);
        _visited.Add(coordinate);
        var numSides = 0;
        char type = _map[coordinate.Row][coordinate.Col];
        while (queue.Count > 0)
        {
            var coord = queue.Dequeue();
            plotPoints.Add(coord);
            foreach (var direction in Coordinate.AdjacentNoDiagonal)
            {
                var neighbor = coord + direction;
                if (neighbor.InBounds(_height, _width))
                {
                    if (_map[neighbor.Row][neighbor.Col] == _map[coord.Row][coord.Col])
                    {
                        if (_visited.Contains(neighbor))
                            continue;
                        _visited.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }
            }
        }
        var grouped = plotPoints.GroupBy(pp => pp.Row);
        Console.WriteLine(type);
        PrintGroups(grouped);
        _plots.Add(plotPoints);

        var prevWidthStart = grouped.First().First().Col;
        var prevWidthEnd = grouped.First().Last().Col;
        numSides += 2;
        foreach (var row in grouped.Skip(1))
        {
            // if (row.)
        }
        return (plotPoints.Count, grouped.Count());
    }

    private static void PrintGroups(IEnumerable<IGrouping<int, Coordinate>>? grouped)
    {
        if (grouped is null) return;

        foreach (var group in grouped)
        {
            Console.Write($"Row {group.Key}: [");
            foreach (var item in group)
            {
                Console.Write($"({item.Row},{item.Col})");
            }
            Console.WriteLine("]");
        }
    }
}