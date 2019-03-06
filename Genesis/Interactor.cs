using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.Genesis
{
    public class Interactor
    {
        private static readonly Action Exit = () => { };
        private static readonly Action NoOp = () => { };

        private readonly Grid _grid;
        private readonly IDictionary<ConsoleKey, Command> _commands;

        public Interactor(Grid grid) => (_grid, _commands) = (grid, GetCommands(grid));

        public Point Origin => _grid.Origin + _grid.Size.Y + 1;

        public bool Interact()
        {
            var op = GetOperation();
            if (op == Exit)
                return false;
            op();
            return true;
        }

        public IEnumerable<Action> CommandRenderers
            => _commands.Values.Where(c => c.Render != null).Select(c => c.Render!);

        private IDictionary<ConsoleKey, Command> GetCommands(Grid grid)
            => GetArrowCommands(grid)
            .Concat(GetActionCommands(grid))
            .Concat(GetColorCommands(grid))
            .ToDictionary(c => c.Key, c => c);

        private static Command[] GetArrowCommands(Grid grid)
            => new[] {
            new Command(ConsoleKey.UpArrow, grid.Up),
            new Command(ConsoleKey.DownArrow, grid.Down),
            new Command(ConsoleKey.LeftArrow, grid.Left),
            new Command(ConsoleKey.RightArrow, grid.Right),
        };

        private static Command[] GetActionCommands(Grid grid)
            => new Command[] {
            new Command("_Plot", grid.Plot),
            new Command("_Fill", grid.Fill),
            new Command("E_xit", Exit),
        };

        private IEnumerable<Command> GetColorCommands(Grid grid)
            => grid.Colors.Select(color => CreateColorCommand(grid, color));

        private Command CreateColorCommand(Grid grid, ConsoleColor color)
            => new Command(
                Enum.Parse<ConsoleKey>($"D{(int)color}"),
                () => SetColor(color),
                () => RenderColorCommand(grid, color));

        private void SetColor(ConsoleColor color)
        {
            _grid.SelectedColor = color;
            this.RenderCommands();
        }

        private static void RenderColorCommand(Grid grid, ConsoleColor color)
        {
            if (grid.SelectedColor == color)
                Renderer.SetColor(ConsoleColor.White, ConsoleColor.Black);
            else
                Renderer.ResetColor();
            Console.Write($"{(int)color}. ");
            Renderer.SetColor(color);
            Console.Write($"{color}");
        }

        private Action GetOperation()
            => GetOperation(Console.ReadKey(true));

        private Action GetOperation(ConsoleKeyInfo keyInfo)
            => _commands.TryGetValue(keyInfo.Key, out var command) ? command.Execute : NoOp;
    }
}