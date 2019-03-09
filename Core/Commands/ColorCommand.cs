using ConsoleDraw.Core.Commands.Operations;
using System;

namespace ConsoleDraw.Core
{
    public class ColorCommand : CommandBase
    {
        private readonly Func<bool> _isActiveChecker;
        private readonly ConsoleColor _color;

        public ColorCommand(Grid grid, ConsoleColor color)
            : base(Enum.Parse<ConsoleKey>($"D{(int)color}"), $"{(int)color}", $"{color}")
            => (_color, _isActiveChecker) = (color, () => grid.SelectedColor == color);

        protected override ConsoleColor NameBackground => _color;
        protected override ConsoleColor NameForeground => ConsoleColor.White;
        protected override bool IsActive => _isActiveChecker();

        public override IExecutable CreateOperation(Grid grid) => new ColorOperation(grid, _color);

        private class ColorOperation : Operation
        {
            private readonly ConsoleColor _color;

            public ColorOperation(Grid grid, ConsoleColor color)
                : base(grid) => _color = color;

            public override bool Execute() => Grid.SelectedColor != (Grid.SelectedColor = _color);
        }
    }
}