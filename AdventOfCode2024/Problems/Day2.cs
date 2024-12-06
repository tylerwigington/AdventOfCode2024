using Gee.External.Capstone.XCore;

namespace AdventOfCode2024.Problems;

public static class Day2
{
    private const int SAFE_MIN = 1;
    private const int SAFE_MAX = 3;
    
    public static async Task<int> RunPart1Async()
    {
        using var file = File.OpenText("./Inputs/day2.txt");
        List<List<int>> reports = [];
        do
        {
            var line = await file.ReadLineAsync();
            if(line is null) continue;
            reports.Add(line.Split(' ')
                .Where(c => !string.IsNullOrEmpty(c))
                .Select(int.Parse)
                .ToList());
        } while(!file.EndOfStream);

        var validReports = 0;
        foreach (var report in reports)
        {
            var isDesc = report.First() > report.Last();
            var wasValid = true;
            for (var i = 0; i < report.Count - 1; i++)
            {
                var diff = !isDesc
                    ? report[i + 1] - report[i] 
                    : report[i] - report[i + 1];
                
                if (diff is >= SAFE_MIN and <= SAFE_MAX) continue;
                wasValid = false;
                break;
            }
            validReports += wasValid ? 1 : 0;
        }
        return validReports;
    }

    public static async Task<long> RunPart2Async()
    {
        using var file = File.OpenText("./Inputs/day2.txt");
        List<List<int>> reports = [];
        do
        {
            var line = await file.ReadLineAsync();
            if(line is null) continue;
            reports.Add(line.Split(' ')
                .Where(c => !string.IsNullOrEmpty(c))
                .Select(int.Parse)
                .ToList());
        } while(!file.EndOfStream);
        
        return reports.Select(IsSafe2).Sum();
    }

    private static int IsSafe(List<int> report)
    {
        var ascending = report[0] < report[1];
        var prev = report[0];
        foreach (var curr in report[1..])
        {
            var diff = curr - prev;
            var wrongDirection = (ascending && prev > curr) || (!ascending && prev < curr);
            if (Math.Abs(diff) is < SAFE_MIN or > SAFE_MAX || wrongDirection)
            {
                return 0;
            }
            prev = curr;
        }
        return 1;
    }

    private static int IsSafe2(List<int> report)
    {
        if(IsSafe(report) == 1) return 1;

        for (var skipIndex = 0; skipIndex < report.Count; skipIndex++)
        {
            var subset = new List<int>(report);
            subset.RemoveAt(skipIndex);
            if(IsSafe(subset) == 1) return 1;
        }
        
        return 0;
    }
}