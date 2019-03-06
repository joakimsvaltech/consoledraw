using System;
using System.Collections.Generic;
using System.Linq;

namespace FloodFill
{

    public static class GridRenderer
    {

        public static void RenderRows(this Grid grid)
        {
            for (int y = 0; y < grid.Size.Y; y++)
                PrintGridLine(grid.Row(y));
        }

        public static void RenderCommands(this Grid grid)
        {
            PositionCursor(grid.Origin + grid.Size.Y + 1);
            grid.ColorCommands()
                .Concat(ActionCommands)
                .Interleave(RenderSeparator)
                .ForEach(render => render());
        }

        public static void UpdateMarker(this Grid grid)
        {
            PositionCursor(grid.Origin + grid.CurrentPos);
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

        private static IEnumerable<Action> ColorCommands(this Grid grid) => grid.Colors
                .Select(color => (Action)(() => grid.ColorCommand((int)color, color)));

        private static IEnumerable<Action> ActionCommands => ActionLabels().Select(l => (Action)(() => ActionCommand(l)));

        private static IEnumerable<string> ActionLabels()
        {
            yield return "_Plot";
            yield return "_Fill";
            yield return "E_xit";
        }

        private static void ColorCommand(this Grid grid, int index, ConsoleColor color)
        {
            if (grid.SelectedColor == color)
                Renderer.SetColor(ConsoleColor.White, ConsoleColor.Black);
            else
                Renderer.ResetColor();
            Console.Write($"{index}. ");
            Renderer.SetColor(color);
            Console.Write($"{color}");
        }

        private static void ActionCommand(string label)
        {
            Renderer.ResetColor();
            var tag = char.ToUpper(label[label.IndexOf('_') + 1]);
            var name = label.Replace("_", "");
            Console.Write($"{tag}. {name}");
        }

        private static void RenderSeparator()
        {
            Renderer.ResetColor();
            Console.Write(" | ");
        }

        private static void Plot(this Grid grid, Point pos) => grid.Plot(grid[pos]);

        private static void Plot(this Grid grid, Cell cell)
        {
            PositionCursor(grid.Origin + cell.Pos);
            PrintGridCell(cell);
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