namespace AdventOfCode2024.Problems;

public static class Day9
{
    public static async Task<long> Part1Async()
    {
        using var file = File.OpenText("./Inputs/day9_test.txt");

        var currId = 0;
        List<FileBlock> blocks = [];
        while (!file.EndOfStream)
        {
            var sizeBit = file.Read();
            if (sizeBit == -1) break;

            var size = int.Parse(((char)sizeBit).ToString());
            var free = int.Parse(((char)file.Read()).ToString());
            blocks.Add(new(currId, size, free));
            currId++;
        }

        return blocks.Count;
    }
}

public sealed record FileBlock(int Id, int Length, int FreeSpace);
