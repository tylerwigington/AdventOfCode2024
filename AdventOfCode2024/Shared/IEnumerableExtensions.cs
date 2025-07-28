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
            }
            else
            {
                foreach (var result in GetPermutations(items.Except([item]), count - 1))
                {
                    yield return new T[] { item }.Concat(result);
                }
            }
            ++i;
        }
    }

    public static bool TryGetElementAt<T>(this IEnumerable<T>? input, int index, out T? outRef)
    {
        outRef = default;
        if (input is null) return false;

        var ele = input.ElementAtOrDefault(index);
        if (ele is not null) outRef = ele;
        return ele is not null;
    }
}
