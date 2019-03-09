using ConsoleDraw.Core.Commands.Operations;
using System;

namespace ConsoleDraw.Core
{
    public class ColorCommand : CommandBase
    {
        private readonly Grid _grid;
        private readonly ConsoleColor _color;

        public ColorCommand(Grid grid, ConsoleColor color)
            : base(Enum.Parse<ConsoleKey>($"D{(int)color}"), $"{(int)color}", $"{color}")
        {
            _grid = grid;
            _grid.ColorChanged += Grid_ColorChanged;
            _color = color;
        }

        private void Grid_ColorChanged(object sender, ColorEventArgs e)
        {
            if (_grid.SelectedColor == _color)
                OnActivated();
            else
                OnInactivated();
        }

        public override ConsoleColor NameBackground => _color;
        public override ConsoleColor NameForeground => ConsoleColor.White;
        public override bool IsActive => _grid.SelectedColor == _color;

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