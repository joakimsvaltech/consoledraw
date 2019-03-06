using System.Collections.Generic;
using System.Linq;

namespace FloodFill
{
    public static class GridExtensions {
        public static IEnumerable<TCell> Column<TCell>(this TCell[,] grid, int x)
            => Enumerable.Range(0, grid.GetLength(1)).Select(y => grid[x, y]);

        public static IEnumerable<TCell> Row<TCell>(this TCell[,] grid, int y)
            => Enumerable.Range(0, grid.GetLength(0)).Select(x => grid[x, y]);

        public static void Replace(this int[,] grid, int replace, int with)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
                for (int y = 0; y < grid.GetLength(1); y++)
                    if (grid[x, y] == replace)
                        grid[x, y] = with;
        }

        public static IEnumerable<TCell> Cells<TCell>(this TCell[,] grid)
            => grid.OfType<TCell>();
    }
}
