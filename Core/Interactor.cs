using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.Core
{
    public class Interactor
    {
        private readonly Grid _grid;
        private readonly IDictionary<ConsoleKey, ICommand> _commands;

        public Interactor(Grid grid) => (_grid, _commands) = (grid, GetCommands(grid));

        public Point Origin => _grid.Origin * (_grid.Size.Y + 1);

        public bool Interact()
        {
            var command = GetCommand();
            if (command == null)
                return true;
            if (command is ExitCommand)
                return false;
            _grid.Perform(command.CreateOperation());
            this.RenderCommands();
            return true;
        }

        internal IEnumerable<ICommand> Commands => _commands.Values;

        private IDictionary<ConsoleKey, ICommand> GetCommands(Grid grid)
            => GetArrowCommands(grid)
            .Concat(GetActionCommands(grid))
            .Concat(GetColorCommands(grid))
            .ToDictionary(c => c.Key, c => c);

        private ICommand[] GetArrowCommands(Grid grid)
            => new[] {
            new NavigationCommand(ConsoleKey.UpArrow, grid.Up),
            new NavigationCommand(ConsoleKey.DownArrow, grid.Down),
            new NavigationCommand(ConsoleKey.LeftArrow, grid.Left),
            new NavigationCommand(ConsoleKey.RightArrow, grid.Right)
        };

        private ICommand[] GetActionCommands(Grid grid)
            => new ICommand[] {
            new Plot(grid),
            new ModeCommand("_Draw", ToggleDraw, () => _grid.Mode == DrawMode.Drawing),
            new ModeCommand("_Rectangle", ToggleRectangle, () => IsRectangle),
            new ModeCommand("_Ellipse", ToggleEllipse, () => IsEllipse),
            new ActionCommand("_Fill", grid.Fill),
            new ActionCommand(ConsoleKey.Enter, Enter, "Enter", "Fill shape"),
            new ActionCommand(ConsoleKey.Escape, Escape, "Esc", "Escape"),
            new UndoCommand(Undo),
            new ExitCommand(),
        };

        private void Undo()
        {
            _grid.Undo();
        }

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
        }

        private void Escape()
        {
            _grid.Mode = DrawMode.None;
        }

        private void FillShape()
        {
            _grid.FillShape();
            _grid.Mode = DrawMode.None;
        }

        private void ToggleDraw()
        {
            _grid.ToggleDraw();
        }

        private IEnumerable<ColorCommand> GetColorCommands(Grid grid)
            => grid.Colors.Select(color => CreateColorCommand(grid, color));

        private ColorCommand CreateColorCommand(Grid grid, ConsoleColor color)
            => new ColorCommand(grid, color);

        private ICommand? GetCommand()
            => GetCommand(Console.ReadKey(true));

        private ICommand? GetCommand(ConsoleKeyInfo keyInfo)
            => _commands.TryGetValue(keyInfo.Key, out var command) ? command : null;
    }
}