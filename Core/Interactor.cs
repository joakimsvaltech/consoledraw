using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.Core
{
    public class Interactor
    {
        private readonly Grid _grid;
        private readonly IDictionary<ConsoleKey, ICommand[]> _commands;

        public Interactor(Grid grid)
        {
            _grid = grid;
            _commands = GetCommands(grid);
            Commands.ForEach(c => c.Activated += (o, e) => this.Render());
            Commands.ForEach(c => c.Inactivated += (o, e) => this.Render());
        }

        public Point Origin => _grid.Origin * (_grid.Size.Y + 1);

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

        internal IEnumerable<ICommand> Commands => _commands.Values.SelectMany(c => c);

        private IDictionary<ConsoleKey, ICommand[]> GetCommands(Grid grid)
            => GetArrowCommands(grid)
            .Concat(GetActionCommands(grid))
            .Concat(GetColorCommands(grid))
            .GroupBy(c => c.Key, c => c)
            .ToDictionary(g => g.Key, g => g.ToArray());

        private ICommand[] GetArrowCommands(Grid grid)
            => new[] {
            new NavigationCommand(grid, ConsoleKey.UpArrow, Direction.Up),
            new NavigationCommand(grid, ConsoleKey.DownArrow, Direction.Down),
            new NavigationCommand(grid, ConsoleKey.LeftArrow, Direction.Left),
            new NavigationCommand(grid, ConsoleKey.RightArrow, Direction.Right)
        };

        private ICommand[] GetActionCommands(Grid grid)
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

        private IEnumerable<ColorCommand> GetColorCommands(Grid grid)
            => grid.Colors.Select(color => CreateColorCommand(grid, color));

        private ColorCommand CreateColorCommand(Grid grid, ConsoleColor color)
            => new ColorCommand(grid, color);

        private ICommand? GetCommand()
            => GetCommand(Console.ReadKey(true));

        private ICommand? GetCommand(ConsoleKeyInfo keyInfo)
            => _commands.TryGetValue(keyInfo.Key, out var command) 
            ? command.SingleOrDefault(c => c.Modifiers == keyInfo.Modifiers) : null;
    }
}