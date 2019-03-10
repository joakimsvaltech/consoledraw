using ConsoleDraw.Core.Geometry;
using System;
using System.Collections.Generic;

namespace ConsoleDraw.Core
{
    internal static class CanvasRenderer
    {
        public static void Render(this Canvas grid)
        {
            grid.Positions.ForEach(pos => grid.Render(pos));
        }

        public static void UpdateMarker(this Canvas grid)
        {
            grid.PositionCursor(grid.CurrentPos);
            grid.Mark(grid.CurrentPos);
        }

        public static void RemoveMarker(this Canvas grid)
        {
            grid.Render(grid.CurrentPos);
            grid.Render(grid.CurrentPos.Right);
        }

        public static void Fill(this Canvas grid, IEnumerable<Cell> area)
        {
            area.ForEach(cell => cell.Color = grid.SelectedColor);
            area.ForEach(cell => grid.Render(cell));
        }

        public static void Unmark(this Canvas grid, IShape? shape)
        {
            if (shape is null)
                return;
            foreach (var pos in shape.Outline)
                grid.Render(pos);
        }

        public static void Mark(this Canvas grid, IShape? shape)
        {
            if (shape is null)
                return;
            foreach (var pos in shape.Outline)
                grid.Mark(pos);
        }

        private static void Render(this Canvas grid, Point pos) => grid.Render(grid[pos]);

        public static void Render(this Canvas grid, Cell cell)
        {
            grid.PositionCursor(cell.Pos);
            grid.PrintGridCell(cell);
        }

        private static void PrintGridCell(this Canvas grid, Cell cell) => grid.Render(cell, cell.Color, ConsoleColor.White);

        private static void Mark(this Canvas grid, Point pos) => grid.MarkGridCell(grid[pos]);

        private static void MarkGridCell(this Canvas grid, Cell cell) => grid.Render(cell, ConsoleColor.White, ConsoleColor.Black);

        private static void Render(this Canvas grid, Cell cell, ConsoleColor bg, ConsoleColor fg)
        {
            Console.BackgroundColor = bg;
            Console.ForegroundColor = fg;
            grid.PositionCursor(cell.Pos);
            Console.Write(cell.Tag);
        }

        private static void PositionCursor(this Canvas grid, Point pos)
        {
            Renderer.PositionCursor(grid.Origin + pos);
        }
    }
}