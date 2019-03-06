using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.Genesis
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<TCell> Row<TCell>(this TCell[,] grid, int y)
            => Enumerable.Range(0, grid.GetLength(0)).Select(x => grid[x, y]);

        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (var x in list)
                action(x);
        }

        public static IEnumerable<T> Interleave<T>(this IEnumerable<T> items, T separator)
        {
            var list = items.ToArray();
            if (!list.Any())
                yield break;
            yield return list.First();
            foreach (var item in list.Skip(1))
            {
                yield return separator;
                yield return item;
            }
        }
    }
}
