using ConsoleDraw.Core.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.Core.Interaction
{
    public class Interactor
    {
        private readonly Canvas _grid;
        private readonly IDictionary<ConsoleKey, ICommand[]> _commands;

        public Interactor(Canvas grid)
        {
            _grid = grid;
            _commands = GetCommands(grid);
        }

        public bool Interact()
        {
            var command = GetCommand();
            if (command == null)
                return true;
            if (command is ExitCommand)
                return false;
            _grid.Execute(command);
            return true;
        }

        public IEnumerable<ICommand> Commands => _commands.Values.SelectMany(c => c);

        private IDictionary<ConsoleKey, ICommand[]> GetCommands(Canvas grid)
            => GetArrowCommands(grid)
            .Concat(GetActionCommands(grid))
            .Concat(GetColorCommands(grid))
            .GroupBy(c => c.Key, c => c)
            .ToDictionary(g => g.Key, g => g.ToArray());

        private ICommand[] GetArrowCommands(Canvas grid)
            => new[] {
            new NavigationCommand(grid, ConsoleKey.UpArrow, Direction.Up),
            new NavigationCommand(grid, ConsoleKey.DownArrow, Direction.Down),
            new NavigationCommand(grid, ConsoleKey.LeftArrow, Direction.Left),
            new NavigationCommand(grid, ConsoleKey.RightArrow, Direction.Right)
        };

        private ICommand[] GetActionCommands(Canvas grid)
            => new ICommand[] {
            new Plot(grid),
            new DrawCommand(grid),
            new RectangleCommand(grid),
            new EllipseCommand(grid),
            new LineCommand(grid),
            new FillCommand(grid),
            new ApplyCommand(grid),
            new EscapeCommand(grid),
            new UndoCommand(grid),
            new RedoCommand(grid),
            new ExitCommand(grid),
        };

        private IEnumerable<ColorCommand> GetColorCommands(Canvas grid)
            => grid.Colors.Select(color => CreateColorCommand(grid, color));

        private ColorCommand CreateColorCommand(Canvas grid, ConsoleColor color)
            => new ColorCommand(grid, color);

        private ICommand? GetCommand()
            => GetCommand(Console.ReadKey(true));

        private ICommand? GetCommand(ConsoleKeyInfo keyInfo)
            => _commands.TryGetValue(keyInfo.Key, out var command)
            ? command.SingleOrDefault(c => c.Modifiers == keyInfo.Modifiers) : null;
    }
}