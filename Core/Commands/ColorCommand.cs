using System;

namespace ConsoleDraw.Core
{
    public class ColorCommand : CommandBase
    {
        public ColorCommand(Grid grid, ConsoleColor color)
            : base(Enum.Parse<ConsoleKey>($"D{(int)color}"), $"{(int)color}", $"{color}")
            => (_color, _isActiveChecker) = (color, () => grid.SelectedColor == color);

        private readonly Func<bool> _isActiveChecker;
        private readonly ConsoleColor _color;

        protected override ConsoleColor NameBackground => _color;
        protected override ConsoleColor NameForeground => ConsoleColor.White;
        protected override ConsoleColor TagBackground => _isActiveChecker()
            ? ConsoleColor.White
            : base.TagBackground;

        protected override ConsoleColor TagForeground => _isActiveChecker()
            ? ConsoleColor.Black
            : base.TagForeground;

        public override IOperation CreateOperation(Grid grid) => new ColorOperation(grid, _color);

        private class ColorOperation : Operation
        {
            private readonly ConsoleColor _color;

            public ColorOperation(Grid grid, ConsoleColor color)
                : base(grid) => _color = color;

            public override void Execute() => Grid.SelectedColor = _color;
        }
    }
}