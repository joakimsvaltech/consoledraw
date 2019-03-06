using System;
using System.Collections.Generic;

namespace FloodFill
{
    public static class GridRenderer {

        public static void RenderRows(this Grid grid)
        {
            for (int y = 0; y < grid.Size.Y; y++)
                PrintGridLine(grid.Row(y));
        }

        public static void UpdateMarker(this Grid grid)
        {
            PositionCursor(grid.Origin + grid.Current);
            MarkGridCell(grid[grid.Current]);
        }

        public static void RemoveMarker(this Grid grid)
        {
            grid.Plot(grid.Current);
            grid.Plot(grid.Current.Right);
        }

        private static void Plot(this Grid grid, Point pos)
        {
            PositionCursor(grid.Origin + pos);
            PrintGridCell(grid[pos]);
        }

        private static void PrintGridLine(IEnumerable<Cell> row)
        {
            foreach (var cell in row)
                PrintGridCell(cell);
            Console.WriteLine();
        }

        private static void PositionCursor(Point absPos)
        {
            Console.SetCursorPosition(absPos.X, absPos.Y);
        }

        private static void PrintGridCell(Cell cell) => Render(cell, (ConsoleColor)cell.ColorIndex, ConsoleColor.White);

        private static void MarkGridCell(Cell cell) => Render(cell, ConsoleColor.White, ConsoleColor.Black);

        private static void Render(Cell cell, ConsoleColor bg, ConsoleColor fg)
        {
            Console.BackgroundColor = bg;
            Console.ForegroundColor = fg;
            Console.Write(cell.Tag);
        }
    }
}