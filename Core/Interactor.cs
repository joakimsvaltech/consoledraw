using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.Core
{
    public class Interactor
    {
        private static readonly Action Exit = () => { };
        private static readonly Action NoOp = () => { };

        private readonly Grid _grid;
        private readonly IDictionary<ConsoleKey, Command> _commands;

        public Interactor(Grid grid) => (_grid, _commands) = (grid, GetCommands(grid));

        public Point Origin => _grid.Origin * (_grid.Size.Y + 1);

        public bool Interact()
        {
            var op = GetOperation();
            if (op == Exit)
                return false;
            op();
            return true;
        }

        internal IEnumerable<Command> Commands => _commands.Values;

        private IDictionary<ConsoleKey, Command> GetCommands(Grid grid)
            => GetArrowCommands(grid)
            .Concat(GetActionCommands(grid))
            .Concat(GetColorCommands(grid))
            .ToDictionary(c => c.Key, c => c);

        private Command[] GetArrowCommands(Grid grid)
            => new[] {
            new Command(ConsoleKey.UpArrow, grid.Up),
            new Command(ConsoleKey.DownArrow, grid.Down),
            new Command(ConsoleKey.LeftArrow, grid.Left),
            new Command(ConsoleKey.RightArrow, grid.Right)
        };

        private Command[] GetActionCommands(Grid grid)
            => new Command[] {
            new Command("_Plot", grid.Plot),
            new Command("_Draw", () => Draw(), () => _grid.IsDrawing),
            new Command("_Rectangle", () => Rectangle(), () => _grid.ActiveRectangle != null),
            new Command("_Fill", grid.Fill),
            new Command("E_xit", Exit),
        };

        private void Rectangle()
        {
            if (_grid.ActiveRectangle == null)
                CreateRectangle();
            else
                FillRectangle();
        }

        private void CreateRectangle()
        {
            _grid.ActiveRectangle = new Rectangle(_grid.CurrentPos, _grid.CurrentPos);
        }

        private void FillRectangle()
        {
            _grid.FillRectangle();
            _grid.ActiveRectangle = null;
        }

        private void Draw()
        {
            _grid.ToggleIsDrawing();
            this.RenderCommands();
        }

        private IEnumerable<Command> GetColorCommands(Grid grid)
            => grid.Colors.Select(color => CreateColorCommand(color));

        private Command CreateColorCommand(ConsoleColor color)
            => new Command(
                Enum.Parse<ConsoleKey>($"D{(int)color}"),
                () => SetColor(color),
                $"{(int)color}",
                () => RenderColorCommand(color),
                () => _grid.SelectedColor == color);

        private void SetColor(ConsoleColor color)
        {
            _grid.SelectedColor = color;
            this.RenderCommands();
        }

        private static void RenderColorCommand(ConsoleColor color)
        {
            Renderer.SetColor(color);
            Console.Write($". {color}");
        }

        private Action GetOperation()
            => GetOperation(Console.ReadKey(true));

        private Action GetOperation(ConsoleKeyInfo keyInfo)
            => _commands.TryGetValue(keyInfo.Key, out var command) ? command.Execute : NoOp;
    }
}