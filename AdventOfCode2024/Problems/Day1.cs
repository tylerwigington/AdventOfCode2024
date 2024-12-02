namespace AdventOfCode2024.Problems;

public static class Day1
{
    public static async Task<long> RunPart1Async()
    {
        using var file = File.OpenText("./Inputs/day1.txt");
        List<long> left = [];
        List<long> right = [];
        
        do
        {
            var line = await file.ReadLineAsync();
            if (string.IsNullOrEmpty(line)) continue;
            var split = line.Split(' ');
            split = split.Where(i => !string.IsNullOrEmpty(i)).ToArray();
            _ = long.TryParse(split[0], out var leftValue);
            _ = long.TryParse(split[1], out var rightValue);
            left.Add(leftValue);
            right.Add(rightValue);
        } while(!file.EndOfStream);

        left = left.Order().ToList();
        right = right.Order().ToList();
        return left.Select((t, i) => Math.Abs(t - right[i])).Sum();
    }

    public static async Task<long> RunPart2Async()
    {
        using var file = File.OpenText("./Inputs/day1.txt");
        List<long> left = [];
        List<long> right = [];
        do
        {
            var line = await file.ReadLineAsync();
            if (string.IsNullOrEmpty(line)) continue;
            var split = line.Split(' ').Where(s => !string.IsNullOrEmpty(s)).ToArray();
            _ = long.TryParse(split[0], out var leftValue);
            _ = long.TryParse(split[1], out var rightValue);
            left.Add(leftValue);
            right.Add(rightValue);
        } while(!file.EndOfStream);
        left = left.Order().ToList();
        right = right.Order().ToList();

        Dictionary<long, DuplicateRecord> map = [];
        var searchIndex = 0;
        for (var i = 0; i < left.Count; i++)
        {
            if (!map.ContainsKey(left[i]))
                map.Add(left[i], new DuplicateRecord());

            map[left[i]].LeftCount++;
            
            while(searchIndex < right.Count && right[searchIndex] <= left[i])
            {
                if (right[searchIndex] == left[i])
                {
                    map[left[i]].RightCount++;
                }
                searchIndex++;
            }
        }
        
        var sum = 0L;
        foreach (var record in map)
        {
            if (record.Value.RightCount == 0) continue;
            
            var initial = record.Key * record.Value.RightCount;
            var subsequent = initial * (record.Value.LeftCount - 1);
            sum += initial + subsequent;
        }
        return sum;
    }
}

public sealed class DuplicateRecord
{
    public int LeftCount { get; set; }
    public int RightCount { get; set; }
};