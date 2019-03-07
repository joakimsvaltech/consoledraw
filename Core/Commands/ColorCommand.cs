using System;

namespace ConsoleDraw.Core
{
    public class ColorCommand : CommandBase
    {
        public ColorCommand(Grid grid, ConsoleColor color)
            : base(Enum.Parse<ConsoleKey>($"D{(int)color}"), $"{(int)color}", $"{color}")
            => (_grid, _color, _isActiveChecker) = (grid, color, () => _grid.SelectedColor == color);

        private readonly Func<bool> _isActiveChecker;
        private readonly Grid _grid;
        private readonly ConsoleColor _color;

        protected override ConsoleColor NameBackground => _color;
        protected override ConsoleColor NameForeground => ConsoleColor.White;
        protected override ConsoleColor TagBackground => _isActiveChecker()
            ? ConsoleColor.White
            : base.TagBackground;

        protected override ConsoleColor TagForeground => _isActiveChecker()
            ? ConsoleColor.Black
            : base.TagForeground;

        public override IOperation CreateOperation() => new ColorOperation(_grid, _color);

        private class ColorOperation : IOperation
        {
            private readonly Grid _grid;
            private readonly ConsoleColor _color;

            public ColorOperation(Grid grid, ConsoleColor color)
            {
                _grid = grid;
                _color = color;
            }

            public bool CanUndo => false;

            public void Execute() => _grid.SelectedColor = _color;

            public void Undo()
            {
                throw new NotImplementedException();
            }
        }
    }
}