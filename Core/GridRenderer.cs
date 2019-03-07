using System;
using System.Collections.Generic;

namespace ConsoleDraw.Core
{
    internal static class GridRenderer
    {
        public static void Render(this Grid grid)
        {
            grid.Positions.ForEach(pos => grid.Plot(pos));
        }

        public static void UpdateMarker(this Grid grid)
        {
            grid.PositionCursor(grid.CurrentPos);
            grid.Mark(grid.CurrentPos);
        }

        public static void RemoveMarker(this Grid grid)
        {
            grid.Plot(grid.CurrentPos);
            grid.Plot(grid.CurrentPos.Right);
        }

        public static void Fill(this Grid grid, IEnumerable<Cell> area)
        {
            area.ForEach(cell => cell.Color = grid.SelectedColor);
            area.ForEach(cell => grid.Plot(cell));
        }

        public static void Unmark(this Grid grid, IShape? shape)
        {
            if (shape == null)
                return;
            foreach (var pos in shape.Outline)
                grid.Plot(pos);
        }

        public static void Mark(this Grid grid, IShape? shape)
        {
            if (shape == null)
                return;
            foreach (var pos in shape.Outline)
                grid.Mark(pos);
        }

        public static void Plot(this Grid grid, Point pos) => grid.Plot(grid[pos]);

        private static void Plot(this Grid grid, Cell cell)
        {
            grid.PositionCursor(cell.Pos);
            grid.PrintGridCell(cell);
        }

        private static void PrintGridCell(this Grid grid, Cell cell) => grid.Render(cell, cell.Color, ConsoleColor.White);

        private static void Mark(this Grid grid, Point pos) => grid.MarkGridCell(grid[pos]);

        private static void MarkGridCell(this Grid grid, Cell cell) => grid.Render(cell, ConsoleColor.White, ConsoleColor.Black);

        private static void Render(this Grid grid, Cell cell, ConsoleColor bg, ConsoleColor fg)
        {
            Console.BackgroundColor = bg;
            Console.ForegroundColor = fg;
            grid.PositionCursor(cell.Pos);
            Console.Write(cell.Tag);
        }

        private static void PositionCursor(this Grid grid, Point pos)
        {
            Renderer.PositionCursor(grid.Origin + pos);
        }
    }
}