using System.Diagnostics;

namespace AdventOfCode2024.Problems;

public static class Day11
{
    public static async Task<long> Part1Async()
    {
        using var file = File.OpenText("./Inputs/day11.txt");
        var input = await file.ReadLineAsync();
        var stones = input!.Split(" ").Select(long.Parse);
        var cache = new Dictionary<(long num, int depth), long>();
        long sum = 0;
        foreach (var stone in stones)
        {
            sum += Blink(stone, 75, cache);
        }
        return sum;
    }

    private static long Blink(long stone, int depth, Dictionary<(long num, int depth), long> cache)
    {
        if (cache.TryGetValue((stone, depth), out var count))
        {
            return count;
        }

        if (depth == 0) return 1;

        long newCount;
        if (stone == 0)
        {
            newCount = Blink(1, depth - 1, cache);
        }
        else if (stone.ToString().Length % 2 == 0)
        {
            var stoneS = stone.ToString();
            newCount = Blink(long.Parse(stoneS[..(stoneS.Length / 2)]), depth - 1, cache)
                + Blink(long.Parse(stoneS[(stoneS.Length / 2)..]), depth - 1, cache);
        }
        else
        {
            newCount = Blink(stone * 2024, depth - 1, cache);
        }
        cache.Add((stone, depth), newCount);
        return newCount;
    }
}