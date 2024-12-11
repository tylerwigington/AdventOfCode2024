using System.Text;

namespace AdventOfCode2024.Problems;

public static class Day9
{
    public static async Task<long> Part1Async()
    {
        using var file = File.OpenText("./Inputs/day9_test.txt");

        //using var ms = new MemoryStream();
        //await ms.WriteAsync(Encoding.UTF8.GetBytes("12345"));
        //ms.Seek(0, SeekOrigin.Begin);
        //using var file = new StreamReader(ms);
        var currId = 0;
        List<FileBlock> blocks = [];
        List<int> fileBlocks = [];
        while (!file.EndOfStream)
        {
            var sizeBit = file.Read();
            if (sizeBit == -1) break;

            var size = (int)char.GetNumericValue((char)sizeBit);

            var freeBit = file.Read();
            if (freeBit == -1)
            {
                fileBlocks.AddRange(Enumerable.Repeat(currId, size));
                break;
            }

            var free = (int)char.GetNumericValue((char)freeBit);
            fileBlocks.AddRange(Enumerable.Repeat(currId, size).Concat(Enumerable.Repeat(-1, free)));
            //blocks.Add(new() { Id = currId, Size = size, FreeSpace = free });
            currId++;
        }

        var forwardIndex = 0;
        var reverseIndex = fileBlocks.Count - 1;
        while (reverseIndex >= forwardIndex)
        {
            while (fileBlocks[forwardIndex] != -1)
            {
                forwardIndex++;
            }

            while (fileBlocks[forwardIndex] == -1)
            {
                fileBlocks[forwardIndex] = fileBlocks[reverseIndex];
                forwardIndex++;
                reverseIndex--;
            }
        }

        var sum = 0;
        for (var i = 0; i < fileBlocks.Count - 1; i++)
        {
            sum += fileBlocks[i] * i;
        }
        return sum;

        //List<FileBlock> sorted = [];
        //FileBlock? curr;
        //var lastQueue = new Queue<FileBlock>();
        //lastQueue.Enqueue(blocks.Last());
        //blocks.RemoveAt(blocks.Count - 1);
        //while (blocks.Count > 0)
        //{
        //    curr = blocks.First();
        //    var currLast = lastQueue.Peek();
        //    while (!curr.Full)
        //    {
        //        if(currLast.Size - currLast.SizeUsed == 0)
        //        {
        //            lastQueue.Dequeue();
        //            var newLast = blocks.Last();
        //            lastQueue.Enqueue(newLast);
        //            currLast = newLast;
        //            blocks.RemoveAt(blocks.Count - 1);
        //        }

        //        var amountToFill = curr.FreeSpace - curr.FilledSpace.Count;
        //        curr.FilledSpace.AddRange(Enumerable.Repeat(new FillBlock(currLast.Id), amountToFill));
        //        currLast.SizeUsed += amountToFill;
        //    }
        //    sorted.Add(curr);
        //    blocks.RemoveAt(0);
        //}

        //int sum = 0;
        //var temp = sorted.SelectMany(s => Enumerable.Repeat(new FillBlock(s.Id), s.Size).Concat(s.FilledSpace)).ToList();
        //for(int i = 0; i < temp.Count; i++)
        //{
        //    sum += i * temp[i].BlockId;
        //}
        //return sum;
    }
}

public sealed class FileBlock
{
    public required int Id { get; init; }
    public required int Size { get; init; }
    public required int FreeSpace { get; init; }
    public int SizeUsed { get; set; }
    public List<FillBlock> FilledSpace { get; set; } = [];
    public bool Full => FilledSpace.Count == FreeSpace;
}

public sealed record FillBlock(int BlockId);
