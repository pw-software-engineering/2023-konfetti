namespace CrossTeamTestSuite.Extensions;

public static class IEnumerableExtenions
{
    public static IEnumerable<(T Item, int Index)> WithIndex<T>(this IEnumerable<T> items)
    {
        return items.Select((item, index) => (item, index));
    }
}
