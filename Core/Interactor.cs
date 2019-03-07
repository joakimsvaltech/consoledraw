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
            new Command("_Draw", ToggleDraw, () => _grid.Mode == DrawMode.Drawing),
            new Command("_Rectangle", ToggleRectangle, () => IsRectangle),
            new Command("_Ellipse", ToggleEllipse, () => IsEllipse),
            new Command("_Fill", grid.Fill),
            new Command(ConsoleKey.Enter, Enter, "Enter", () => Render("Fill shape")),
            new Command(ConsoleKey.Escape, Escape, "Esc", () => Render("Escape")),
            new Command("E_xit", Exit),
        };
        
        private void ToggleRectangle()
        {
            if (IsRectangle)
                FillShape();
            else
                _grid.Mode = DrawMode.Rectangle;
            this.RenderCommands();
        }

        private void ToggleEllipse()
        {
            if (IsEllipse)
                FillShape();
            else
                _grid.Mode = DrawMode.Ellipse;
            this.RenderCommands();
        }

        private bool IsRectangle => _grid.Mode == DrawMode.Rectangle;
        private bool IsEllipse => _grid.Mode == DrawMode.Ellipse;

        private void Enter()
        {
            FillShape();
            this.RenderCommands();
        }

        private void Escape()
        {
            _grid.Mode = DrawMode.None;
            this.RenderCommands();
        }

        private void FillShape()
        {
            _grid.FillShape();
            _grid.Mode = DrawMode.None;
        }

        private void ToggleDraw()
        {
            _grid.ToggleDraw();
            this.RenderCommands();
        }

        private IEnumerable<Command> GetColorCommands(Grid grid)
            => grid.Colors.Select(color => CreateColorCommand(color));

        private Command CreateColorCommand(ConsoleColor color)
            => new Command(
                Enum.Parse<ConsoleKey>($"D{(int)color}"),
                () => SetColor(color),
                $"{(int)color}",
                () => RenderColorCommandName(color),
                () => _grid.SelectedColor == color);

        private void SetColor(ConsoleColor color)
        {
            _grid.SelectedColor = color;
            this.RenderCommands();
        }

        private static void RenderColorCommandName(ConsoleColor color)
        {
            Renderer.SetColor(color);
            Render($"{color}");
        }

        private static void Render(string text)
        {
            Console.Write(text);
        }

        private Action GetOperation()
            => GetOperation(Console.ReadKey(true));

        private Action GetOperation(ConsoleKeyInfo keyInfo)
            => _commands.TryGetValue(keyInfo.Key, out var command) ? command.Execute : NoOp;
    }
}