namespace AdventOfCode2024.Shared;

public interface IPuzzleSolution<T>
{
    public abstract Task<T> SolveAsync();
}