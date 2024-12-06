namespace AdventOfCode2024.Problems;

using Occurence = (int StartX, int StartY, (int X, int Y) Direction);

public sealed class Day4
{
    private static readonly List<string> _input = [];
    private const string _word = "XMAS";
    private const string _part2Word = "MAS";
    private static readonly List<(int, int)> _part1Directions = 
    [
        (-1,1),(0,1),(1,1),
        (-1,0),(1,0),
        (-1,-1),(0,-1),(1,-1)
    ];
    private static readonly List<(int, int)> _part2Directions =
    [
        (-1,1),(1,1),
        (-1,-1),(1,-1)
    ];
    
    private Day4() { }

    public static async Task<Day4> Create()
    {
        if (_input.Count != 0) return new Day4();
        
        using var file = File.OpenText("./Inputs/day4.txt");
        do
        {
            var line = await file.ReadLineAsync();
            _input.Add(line!);
        } while (!file.EndOfStream);
        return new Day4();
    }

    public int Part1()
    {
        var matrix = _input.Select(line => line.ToList()).ToList();
        var occurrences = FindAllOccurrences(matrix, _word, _part1Directions);
        return occurrences.Count;
    }

    public int Part2()
    {
        var matrix = _input.Select(line => line.ToList()).ToList();
        var occurrences = FindAllOccurrences(matrix, _part2Word, _part2Directions);
        var valid = FindValidOccurrences(occurrences);
        return valid.Count;
    }
    
    private static List<Occurence> FindAllOccurrences(List<List<char>> matrix, string word, List<(int,int)> directions)
    {
        var rows = matrix.Count;
        var cols = matrix[0].Count;
        List<Occurence> occurrences = [];
        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                if (matrix[i][j] != word[0]) continue;
                foreach(var (dx, dy) in directions)
                {
                    if (WordExists(matrix, word, i, j, dx, dy))
                    {
                        occurrences.Add((i, j, (dx, dy)));
                    }
                }
            }
        }

        return occurrences;
    }

    private static bool WordExists(List<List<char>> matrix, string word, int startX, int startY, int dx, int dy)
    {
        var rows = matrix.Count;
        var cols = matrix[0].Count;
        var x = startX;
        var y = startY;

        foreach (var t in word)
        {
            if (x < 0 || x >= rows || y < 0 || y >= cols || matrix[x][y] != t)
                return false;

            x += dx;
            y += dy;
        }

        return true;
    }

    private static List<(Occurence, Occurence)> FindValidOccurrences(List<Occurence> occurrences)
    {
        List<(Occurence, Occurence)> valid = [];
        for (var i = 0; i < occurrences.Count; i++)
        {
            var occ = occurrences[i];
            foreach (var compare in occurrences[i..].Skip(1))
            {
                if (OccurrencesCross(occ, compare))
                    valid.Add((occ, compare));
            }
        }
        return valid;
    }
    
    private static bool OccurrencesCross(Occurence a, Occurence b)
    {
        var aCenter = (a.StartX + a.Direction.X, a.StartY + a.Direction.Y);
        var bCenter = (b.StartX + b.Direction.X, b.StartY + b.Direction.Y);
        return aCenter == bCenter;
    }
}