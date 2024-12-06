using System.Text.RegularExpressions;

namespace AdventOfCode2024.Problems;

public partial class Day3Better
{
    private static string _input = string.Empty;

    private static readonly Regex IsDigit = GetIsDigitRegex();
    private static readonly Regex InstructionRegex = GetInstructionRegex();
    private static readonly Regex InstructionCommandRegex = GetInstructionCommandRegex();
    
    private const string InstructionCommand = "mul";
    private const char InstructionStart = '(';
    private const char InstructionEnd = ')';
    private const char InstructionSeparator = ',';
    private const int WindowSize = 2;
    private const int InstructionOffset = 3;
    private const string Do = "do()";
    private const string Dont = "don\'t()";

    private Day3Better() { }
    
    public static async Task<Day3Better> Create()
    {
        if (!string.IsNullOrEmpty(_input)) return new Day3Better();
        
        using var file = File.OpenText("./Inputs/day3.txt");
        var fileText = await file.ReadToEndAsync();
        _input = Do + fileText + Dont;
        return new Day3Better();
    }

    public long Split()
    {
        var donts = _input.Split(Dont);
        var beginningPairs = ParseInstructions(donts[0]);
        return beginningPairs.Concat(donts[1..]
                .SelectMany(d => d.Split(Do)[1..])
                .Select(ParseInstructions)
                .SelectMany(x => x))
                .Select(item => item.Item1 * item.Item2)
                .Sum();
    }

    public long RunRegex()
    {
        var matches = InstructionRegex.Matches(_input);
        var enabled = false;
        long total = 0;
        foreach (Match match in matches)
        {
            var isDo = match.Groups.Values.Any(c => c.Value == Do);
            var isDont = match.Groups.Values.Any(c => c.Value == Dont);
            if (isDo || isDont)
            {
                enabled = isDo;
                continue;
            }

            if (!enabled) continue;
            
            var a = int.Parse(match.Groups[1].Value);
            var b = int.Parse(match.Groups[2].Value);
            total += a * b;
        }
        return total;
    }
    
    private static List<(long, long)> ParseInstructions(string corruptedInstructions)
    {
        List<(long, long)> instructions = [];
        var i = 0;
        while(i < corruptedInstructions.Length - WindowSize)
        {
            // range syntax not inclusive
            var window = corruptedInstructions[i..(i + WindowSize + 1)];
            if (InstructionCommandRegex.IsMatch(window))
            {
                i++;
                continue;
            }

            if (corruptedInstructions.ElementAtOrDefault(i + InstructionOffset) != InstructionStart)
            {
                i += InstructionOffset;
                continue;
            }

            var iStart = i + InstructionOffset + 1;
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

    [GeneratedRegex(@"mul\((\d+),(\d+)\)|(do\(\))|(don't\(\))")]
    private static partial Regex GetInstructionRegex();
    [GeneratedRegex(@"^\d+$")]
    private static partial Regex GetIsDigitRegex();
    [GeneratedRegex(@"mul")]
    private static partial Regex GetInstructionCommandRegex();
}