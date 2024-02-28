using System.Runtime.CompilerServices;

namespace CarSnapScore.Services;

public static class ShuffleListExtension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Shuffle<T>(this IList<T> list)
    {
        ArgumentNullException.ThrowIfNull(list);
        int n = list.Count;
        while (n > 1)
        {
            int k = Random.Shared.Next(n--);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }
}
