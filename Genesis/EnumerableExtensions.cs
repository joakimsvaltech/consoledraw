using System;
using System.Collections.Generic;
using System.Linq;

namespace FloodFill
{
    public static class EnumerableExtensions {
        public static IEnumerable<TCell> Row<TCell>(this TCell[,] grid, int y)
            => Enumerable.Range(0, grid.GetLength(0)).Select(x => grid[x, y]);

        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (var x in list)
                action(x);
        }
    }
}
