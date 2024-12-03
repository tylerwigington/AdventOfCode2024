using AdventOfCode2024;
using BenchmarkDotNet.Running;

// var day1p1 = await Day1.RunPart1Async();
// Console.WriteLine("Day 1 Part 1: " + day1p1);
// var day1p2 = await Day1.RunPart2Async();
// Console.WriteLine("Day 1 Part 2: " + day1p2);
// var day2p1 = await Day2.RunPart1Async();
// Console.WriteLine("Day 2 Part 1: " + day2p1);
// var day2p2 = await Day2.RunPart2Async();
// Console.WriteLine("Day 2 Part 2: " + day2p2);
// var day3p1 = await Day3.RunPart1Async();
// Console.WriteLine("Day 3 Part 1: " + day3p1);
// var day3p2 = await Day3.RunPart2Async();
// Console.WriteLine("Day 3 Part 2: " + day3p2);
// var day3 = (await Day3Better.Create()).RunRegex();
// Console.WriteLine(day3);
var summary = BenchmarkRunner.Run<Benchmarks>();