using System.Diagnostics;

namespace AdventOfCode2024.Problems;

public sealed record Equation(long Value, int[] Nums);

public static class Day7
{
    public static async Task<long> Part1Async()
    {
        using var file = File.OpenText("./Inputs/day7_test.txt");

        var equations = new List<Equation>();
        while (!file.EndOfStream)
        {
            var line = await file.ReadLineAsync();
            if (string.IsNullOrEmpty(line)) continue;
            var parts = line.Split(':', StringSplitOptions.RemoveEmptyEntries);
            equations.Add(new(long.Parse(parts[0]), parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray()));
        }

        var total = 0L;
        foreach (var equation in equations)
        {
            Debug.WriteLine("Equation Val = " + equation.Value);
            if (IsPossible(equation.Value, equation.Nums, false))
                total += equation.Value;
            else if(IsPossible(equation.Value, equation.Nums))
                total += equation.Value;
        }
        return total;
    }

    private static bool IsPossible(long testVal, int[] numbers, bool checkConcat = true)
    {
        var tail = numbers.LastOrDefault();
        var theRest = numbers[Range.EndAt(numbers.Length - 1)];
        if (theRest is null || theRest.Length == 0) return tail == testVal;
        var div = Math.DivRem(testVal, tail, out var rem);
        if(rem == 0 && IsPossible(div, theRest)) return true;
        if(checkConcat 
            && EndsWith(testVal, tail) 
            && IsPossible((long)Math.Floor(testVal /  Math.Pow(10, GetDigitPow(tail))), theRest, checkConcat)) return true;

        return IsPossible(testVal - tail, theRest);
    }

    private static bool EndsWith(long a, int b) => (a - b) % Math.Pow(10, GetDigitPow(b)) == 0;
    //{
    //    var ab = a - b;
    //    var pow = Math.Pow(10, GetDigitPow(b));
    //    var mod = ab % pow;
    //    Debug.WriteLine(string.Format("A - B = {0}\tPow = {1}\tMod = {2}", ab, pow, mod));
    //    return mod == 0;

    //    return 
    //}

    private static int GetDigitPow(int n) => (int)(Math.Log10(n) + 1);
}
