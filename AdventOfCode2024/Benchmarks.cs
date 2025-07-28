using AdventOfCode2024.Problems;
using BenchmarkDotNet.Attributes;

namespace AdventOfCode2024;


public class Benchmarks
{
    private Day3Better? _day3Better;

    [GlobalSetup]
    public async Task Setup()
    {
        _day3Better = await Day3Better.Create();
    }

    [Benchmark]
    public long Split()
    {
        return _day3Better!.Split();
    }

    [Benchmark]
    public long Regex()
    {
        return _day3Better!.RunRegex();
    }
}