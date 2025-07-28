using System.Text;

namespace AdventOfCode2024.Problems;

public static class Day9
{
    private const int _freeSpace = -1;
    public static async Task<long> Part1Async()
    {
        using var file = File.OpenText("./Inputs/day9.txt");

        var currId = 0;
        List<FileBlock> blocks = [];
        while (!file.EndOfStream)
        {
            var sizeChar = file.Read();
            if (sizeChar == -1) break;

            var fileSize = (int)char.GetNumericValue((char)sizeChar);

            var freeChar = file.Read();
            if (freeChar == -1)
            {
                blocks.Add(new() { Id = currId, Size = fileSize, FreeSpace = 0 });
                break;
            }

            var freeSpace = (int)char.GetNumericValue((char)freeChar);
            blocks.Add(new() { Id = currId, Size = fileSize, FreeSpace = freeSpace });
            currId++;
        }

        List<FileBlock> sorted = [];
        var lastQueue = new Queue<FileBlock>();
        lastQueue.Enqueue(blocks[^1]);
        blocks.RemoveAt(blocks.Count - 1);
        while (blocks.Count > 0)
        {
            var first = blocks.First();
            var last = lastQueue.Peek();
            while (!first.Full && blocks.Count > 1)
            {
                if (last.Size - last.SizeMoved == 0)
                {
                    lastQueue.Dequeue();
                    var newLast = blocks.Last();
                    blocks.RemoveAt(blocks.Count - 1);
                    lastQueue.Enqueue(newLast);
                    last = lastQueue.Peek();
                }

                var available = Math.Min(first.FreeSpace - first.FilledBlocks.Count, last.Size - last.SizeMoved);
                first.FilledBlocks.AddRange(Enumerable.Repeat(last.Id, available));
                last.SizeMoved += available;
            }
            sorted.Add(first);
            blocks.Remove(first);
        }

        if (lastQueue.TryDequeue(out var lastBlock))
        {
            sorted.Add(new FileBlock
            {
                Id = lastBlock.Id,
                Size = lastBlock.Size - lastBlock.SizeMoved,
                FreeSpace = lastBlock.FreeSpace - lastBlock.SizeMoved,
                SizeMoved = lastBlock.SizeMoved
            });
        }

        long sum = 0;
        var temp = sorted.SelectMany(s => Enumerable.Repeat(s.Id, s.Size).Concat(s.FilledBlocks)).ToList();
        for (var i = 0; i < temp.Count; i++)
        {
            sum += i * temp[i];
        }
        return await Task.FromResult(sum);
    }

    public static async Task<long> Part2Async()
    {
        using var file = File.OpenText("./Inputs/day9_test.txt");

        // using var ms = new MemoryStream();
        // await ms.WriteAsync("54321"u8.ToArray());
        // ms.Seek(0, SeekOrigin.Begin);
        // using var file = new StreamReader(ms);

        var currId = 0;
        List<int> blocks = [];
        List<FileBlock> fileBlocks = [];
        while (!file.EndOfStream)
        {
            var sizeChar = file.Read();
            if (sizeChar == -1) break;

            var fileSize = (int)char.GetNumericValue((char)sizeChar);

            var freeChar = file.Read();
            if (freeChar == -1)
            {
                blocks.AddRange(Enumerable.Repeat(currId, fileSize));
                fileBlocks.Add(new() { Id = currId, Size = fileSize, FreeSpace = 0 });
                break;
            }

            var freeSpace = (int)char.GetNumericValue((char)freeChar);
            blocks.AddRange(Enumerable.Repeat(currId, fileSize).Concat(Enumerable.Repeat(_freeSpace, freeSpace)));
            fileBlocks.Add(new() { Id = currId, Size = fileSize, FreeSpace = freeSpace });
            currId++;
        }

        var last = fileBlocks.Count - 1;

        while (last >= 0)
        {
            var lastBlock = fileBlocks[last];
            var front = 0;
            while (front <= last)
            {
                var frontBlock = fileBlocks[front++];
                if (frontBlock.Full || frontBlock.FreeSpace - frontBlock.FilledBlocks.Count < lastBlock.Size) continue;

                frontBlock.FilledBlocks.AddRange(Enumerable.Repeat(lastBlock.Id, lastBlock.Size));
                lastBlock.SizeMoved += lastBlock.Size;
                break;
            }
            last--;
        }

        var flattened = fileBlocks.SelectMany(s => s.SizeMoved == s.Size ? s.FilledBlocks : Enumerable.Repeat(s.Id, s.Size).Concat(s.FilledBlocks)).ToList();
        long sum = 0;
        for (var i = 0; i < flattened.Count; i++)
        {
            if (flattened[i] != _freeSpace)
                sum += flattened[i] * i;
        }
        return await Task.FromResult(sum);
    }

    private static (int StartIndex, int EndIndex) FindLeftMostFreeBlock(List<int> blocks, int size)
    {
        var start = 0;
        while (true)
        {
            while (blocks[start] != _freeSpace && start < blocks.Count - 1) start++;

            var end = start - 1;
            while (blocks[end] == _freeSpace && end < blocks.Count - 1) end++;

            if (start == blocks.Count - 1) return (-1, -1);

            var free = end - start;
            if (free <= size) return (start, end);

            start = end;
        }

    }

    private static (int StartIndex, int EndIndex) FindRightMostBlock(List<int> blocks, int currentIndex)
    {
        var end = currentIndex;
        while (blocks[end] == _freeSpace && end >= 0) end--;

        var start = end;
        while (blocks[start] != _freeSpace && start >= 0) start--;

        if (start == 0 || end == 0) return (-1, -1);

        return (start + 1, end);
    }
}

public sealed class FileBlock
{
    public required int Id { get; init; }
    public required int Size { get; init; }
    public required int FreeSpace { get; init; }
    public int SizeMoved { get; set; }
    public List<int> FilledBlocks { get; } = [];
    public bool Full => FilledBlocks.Count == FreeSpace;
}