using System;
using System.Collections.Generic;

namespace ConsoleDraw.Genesis
{
    public static class GridRenderer
    {
        public static void RenderRows(this Grid grid)
        {
            for (int y = 0; y < grid.Size.Y; y++)
                PrintGridLine(grid.Row(y));
        }

        public static void UpdateMarker(this Grid grid)
        {
            Renderer.PositionCursor(grid.Origin + grid.CurrentPos);
            MarkGridCell(grid[grid.CurrentPos]);
        }

        public static void RemoveMarker(this Grid grid)
        {
            grid.Plot(grid.CurrentPos);
            grid.Plot(grid.CurrentPos.Right);
        }

        public static void Plot(this Grid grid, ConsoleColor color)
        {
            var cell = grid.CurrentCell;
            cell.ColorIndex = (int)color;
            grid.Plot(cell);
        }

        public static void Fill(this Grid grid, IEnumerable<Cell> area)
        {
            area.ForEach(cell => cell.ColorIndex = (int)grid.SelectedColor);
            area.ForEach(cell => grid.Plot(cell));
        }

        private static void Plot(this Grid grid, Point pos) => grid.Plot(grid[pos]);

        private static void Plot(this Grid grid, Cell cell)
        {
            Renderer.PositionCursor(grid.Origin + cell.Pos);
            PrintGridCell(cell);
        }

        private static void PrintGridLine(IEnumerable<Cell> row)
        {
            foreach (var cell in row)
                PrintGridCell(cell);
            Console.WriteLine();
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