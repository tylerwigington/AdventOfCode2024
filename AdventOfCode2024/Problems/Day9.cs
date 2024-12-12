using System.Text;

namespace AdventOfCode2024.Problems;

public static class Day9
{
    public static long Part1()
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
        while (blocks.Count > 0)
        {
            var first = blocks.First();
            var last = lastQueue.Peek();
            while (!first.Full && blocks.Count > 0)
            {
                if(last.Size - last.SizeUsed == 0)
                {
                    lastQueue.Dequeue();
                    var newLast = blocks.Last();
                    lastQueue.Enqueue(newLast);
                    last = newLast;
                    blocks.RemoveAt(blocks.Count - 1);
                    continue;
                }

                var available = first.FreeSpace - first.FreeBlocks.Count;
                if(available > last.Size - last.SizeUsed) available = last.Size - last.SizeUsed;
                first.FreeBlocks.AddRange(Enumerable.Repeat(new FillBlock(last.Id), available));
                last.SizeUsed += available;
            }
            sorted.Add(first);
            blocks.Remove(first);
        }

        if (lastQueue.TryDequeue(out var lastBlock))
        {
            sorted.Add(new FileBlock
            {
                Id = lastBlock.Id, 
                Size = lastBlock.Size - lastBlock.SizeUsed,
                FreeSpace = lastBlock.FreeSpace - lastBlock.SizeUsed,
                SizeUsed = lastBlock.SizeUsed
            });   
        }

        var sum = 0;
        var temp = sorted.SelectMany(s => Enumerable.Repeat(new FillBlock(s.Id), s.Size).Concat(s.FreeBlocks)).ToList();
        for(var i = 0; i < temp.Count; i++)
        {
            sum += i * temp[i].BlockId;
        }
        return sum;
    }
}

public sealed class FileBlock
{
    public required int Id { get; init; }
    public required int Size { get; init; }
    public required int FreeSpace { get; init; }
    public int SizeUsed { get; set; }
    public List<FillBlock> FreeBlocks { get; } = [];
    public bool Full => FreeBlocks.Count == FreeSpace;
}

public sealed record FillBlock(int BlockId);
