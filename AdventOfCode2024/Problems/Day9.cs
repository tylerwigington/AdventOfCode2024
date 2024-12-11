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

            var size = Convert.ToInt32((char)sizeBit);
            var free = Convert.ToInt32((char)file.Read());
            blocks.Add(new(currId, size, free));
            currId++;
        }

        List<FileBlock> sorted = [];
        Dictionary<int, Queue<FileBlock>> blockQueues = new();
        while (blocks.Count > 0)
        {
            var first = blocks.First();
            if (blockQueues.TryGetValue(first.FreeSpace, out var queue) && queue.TryDequeue(out var block))
            {
                sorted.Add(first);
                sorted.Add(block);
                continue;
            }
            
            var last = blocks.LastOrDefault();
            
            if (last.Length == first.FreeSpace)
            {
                sorted.Add(first);
                sorted.Add(last);
                blocks.RemoveAt(blocks.Count - 1);
                continue;
            }
            
            while (last.Length != first.FreeSpace)
            {
                
                if(!blockQueues.ContainsKey(last.Length))
                    blockQueues.Add(last.Length, []);
                blockQueues[last.Length].Enqueue(last);
                blocks.RemoveAt(blocks.Count - 1);
                last = blocks.Last();
            }
            sorted.AddRange([first, last]);
        }

        PrintBlocks(sorted);
        return blocks.Count;
    }

    private static void PrintBlocks(List<FileBlock> blocks)
    {
        foreach (var block in blocks)
        {
            
        }
    }
}

public sealed record FileBlock(int Id, int Length, int FreeSpace);
