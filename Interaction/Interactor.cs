using ConsoleDraw.Core;
using ConsoleDraw.Core.Geometry;
using ConsoleDraw.Core.Interaction;
using ConsoleDraw.Core.Storage;
using ConsoleDraw.Interaction.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.Interaction
{
    public class Interactor
    {
        private readonly Canvas _grid;
        private readonly IRepository _loader;
        private readonly IInput _input;
        private readonly IDictionary<ConsoleKey, ICommand[]> _commands;

        public Interactor(Canvas grid, IRepository loader, IInput input)
        {
            _grid = grid;
            _loader = loader;
            _input = input;
            _commands = GetCommands(grid);
        }

        public bool Interact()
        {
            var command = GetCommand();
            if (command == null)
                return true;
            if (command is Exit)
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
            new Move(grid, ConsoleKey.UpArrow, Direction.Up),
            new Move(grid, ConsoleKey.DownArrow, Direction.Down),
            new Move(grid, ConsoleKey.LeftArrow, Direction.Left),
            new Move(grid, ConsoleKey.RightArrow, Direction.Right)
        };

        private ICommand[] GetActionCommands(Canvas grid)
            => new ICommand[] {
            new Plot(grid),
            new Draw(grid),
            new Rectangle(grid),
            new Ellipse(grid),
            new Line(grid),
            new Fill(grid),
            new RandomFill(grid),
            new Apply(grid),
            new Annotate(grid),
            new Escape(grid),
            new Undo(grid),
            new Redo(grid),
            new Exit(grid),
            new Load(_input, _loader, grid),
            new Save(_input, _loader, grid),
        };

        private IEnumerable<Color> GetColorCommands(Canvas grid)
            => grid.Colors.Select(color => CreateColorCommand(grid, color));

        private Color CreateColorCommand(Canvas grid, ConsoleColor color)
            => new Color(grid, color);

        private ICommand? GetCommand()
            => GetCommand(Console.ReadKey(true));

        private ICommand? GetCommand(ConsoleKeyInfo keyInfo)
            => _commands.TryGetValue(keyInfo.Key, out var commands)
            ? commands.Where(c => c.IsEnabled).SingleOrDefault(c => c.Modifiers == keyInfo.Modifiers) : null;
    }
}