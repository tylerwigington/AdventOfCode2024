namespace AdventOfCode2024.Shared;

public static class IEnumerableExtensions
{
    public static IEnumerable<IEnumerable<T>> GetPermutations<T>(this IEnumerable<T> items, int count)
    {
        var i = 0;
        foreach (var item in items)
        {
            if (count == 1)
            {
                yield return new T[] { item };
            } else
            {
                foreach(var result in GetPermutations(items.Except([item]), count-1))
                {
                    yield return new T[] { item }.Concat(result);
                }
            }
            ++i;
        }
    }
}
