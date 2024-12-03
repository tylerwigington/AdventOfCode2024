namespace AdventOfCode2024.Problems;

public static class Day2
{
    private const int SAFE_MIN = 1;
    private const int SAFE_MAX = 3;

    private enum SortDirection
    {
        Ascending,
        Descending
    }
    
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

    public static async Task<int> RunPart2Async()
    {
        using var file = File.OpenText("./Inputs/day2_test.txt");
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
            // var isSortedAsc = IsSorted(report, SortDirection.Ascending);
            // var isSortedDesc = IsSorted(report, SortDirection.Descending);
            // if (!isSortedAsc && !isSortedDesc) continue;

            var fixedList = report;
            var invalidReport = false;
            for (var i = 0; i < report.Count - 1; i++)
            {
                var curr = report[i];
                var next = report[i + 1];
                var diff = curr - next;
                
                if (Math.Abs(diff) is >= SAFE_MIN and <= SAFE_MAX) continue;
                
                var prev = report[i - 1];
                var removedDiff = prev - next;
                if (Math.Abs(removedDiff) is >= SAFE_MIN and <= SAFE_MAX)
                {
                    fixedList.RemoveAt(i);
                    break;
                };
                invalidReport = true;
                break;
            }
            if(invalidReport) continue;
            validReports += IsSorted(fixedList, SortDirection.Ascending) || IsSorted(fixedList, SortDirection.Descending) ? 1 : 0;
        }
        return validReports;
    }

    private static bool IsSorted(List<int> report, SortDirection validDirection) => validDirection switch
    {
        SortDirection.Ascending => report.Zip(report.Skip(1), (curr, next) => curr <= next).All(x => x),
        SortDirection.Descending => report.Zip(report.Skip(1), (curr, next) => curr >= next).All(x => x),
        _ => false
    };
}