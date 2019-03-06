using System;

namespace FloodFill
{
    public class GridGenerator
    {
        private static readonly Random rand = new Random((int)DateTime.Now.Ticks);

        public static Cell[,] GenerateGrid(int cols, int rows, int colors)
        {
            var grid = new Cell[cols, rows];
            for (int x = 0; x < cols; x++)
                for (int y = 0; y < rows; y++)
                    grid[x, y] = GenerateCell(x, y, colors);
            return grid;
        }

        private static Cell GenerateCell(int x, int y, int colors)
            => new Cell
            {
                Pos = new Point(x, y),
                ColorIndex = rand.Next(colors)
            };
    }
}