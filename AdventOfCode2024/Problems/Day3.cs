using System.Text.RegularExpressions;

namespace AdventOfCode2024.Problems;

public static class Day3
{
    private static Regex Instruction = new(@"mul");
    private static Regex IsDigit = new(@"^\d+$");
    private const char InstructionStart = '(';
    private const char InstructionEnd = ')';
    private const char InstructionSeparator = ',';
    private const int WindowSize = 2;
    private const int InstructionOffSet = 3;
    private const string Do = "do()";
    private const string Dont = "don\'t()";
    
    public static async Task<long> RunPart1Async()
    {
        using var file = File.OpenText("./Inputs/day3.txt");
        var corruptedInstructions = await file.ReadToEndAsync();

        List<(long, long)> instructions = ParseInstructions(corruptedInstructions);
        return instructions.Aggregate(0L, (curr, next) => curr += next.Item1 * next.Item2);
    }
    
    public static async Task<long> RunPart2Async()
    { 
        using var file = File.OpenText("./Inputs/day3.txt");
        var input = await file.ReadToEndAsync();
        var donts = input.Split(Dont);
        var beginningPairs = ParseInstructions(donts[0]);
        return beginningPairs.Concat(donts[1..]
                .SelectMany(d => d.Split(Do)[1..])
                .Select(ParseInstructions)
                .SelectMany(x => x))
                .Select(item => item.Item1 * item.Item2)
                .Sum();
    }

    private static List<(long, long)> ParseInstructions(string corruptedInstructions)
    {
        List<(long, long)> instructions = [];
        var i = 0;
        while(i < corruptedInstructions.Length - WindowSize)
        {
            // range syntax not inclusive
            var window = corruptedInstructions[i..(i + WindowSize + 1)];
            if (!Instruction.IsMatch(window))
            {
                i++;
                continue;
            }

            if (corruptedInstructions.ElementAtOrDefault(i + InstructionOffSet) != InstructionStart)
            {
                i += InstructionOffSet;
                continue;
            }

            var iStart = i + InstructionOffSet + 1;
            var y = iStart;
            while (IsDigit.IsMatch(corruptedInstructions[y].ToString()) || corruptedInstructions[y] == InstructionSeparator)
                y++;

            if (corruptedInstructions[y] != InstructionEnd)
            {
                i += y - i;
                continue;
            }
            
            var pair =  corruptedInstructions[iStart..y].Split(InstructionSeparator);
            if (pair.Length == 2)
                instructions.Add((long.Parse(pair[0]), long.Parse(pair[1])));
            
            i += y - i;
        }
        return instructions;
    }
}