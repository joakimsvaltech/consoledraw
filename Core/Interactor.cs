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
            _grid.Execute(command);
            this.RenderCommands();
            return true;
        }

        internal IEnumerable<ICommand> Commands => _commands.Values;

        private IDictionary<ConsoleKey, ICommand> GetCommands(Grid grid)
            => GetArrowCommands()
            .Concat(GetActionCommands(grid))
            .Concat(GetColorCommands(grid))
            .ToDictionary(c => c.Key, c => c);

        private ICommand[] GetArrowCommands()
            => new[] {
            new NavigationCommand(ConsoleKey.UpArrow, Direction.Up),
            new NavigationCommand(ConsoleKey.DownArrow, Direction.Down),
            new NavigationCommand(ConsoleKey.LeftArrow, Direction.Left),
            new NavigationCommand(ConsoleKey.RightArrow, Direction.Right)
        };

        private ICommand[] GetActionCommands(Grid grid)
            => new ICommand[] {
            new Plot(),
            new DrawCommand(grid),
            new RectangleCommand(grid),
            new EllipseCommand(grid),
            new LineCommand(grid),
            new FillCommand(),
            new ApplyCommand(),
            new EscapeCommand(),
            new UndoCommand(),
            new ExitCommand(),
        };

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