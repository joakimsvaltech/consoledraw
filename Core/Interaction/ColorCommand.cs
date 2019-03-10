using ConsoleDraw.Core.Commands.Operations;
using System;

namespace ConsoleDraw.Core
{
    public class ColorCommand : CommandBase
    {
        private readonly ConsoleColor _color;

        public ColorCommand(Canvas grid, ConsoleColor color)
            : base(grid, Enum.Parse<ConsoleKey>($"D{(int)color}"), $"{(int)color}", $"{color}")
        {
            grid.ColorChanged += Grid_ColorChanged;
            _color = color;
        }

        private void Grid_ColorChanged(object sender, ColorEventArgs e)
        {
            if (Grid.SelectedColor == _color)
                OnActivated();
            else
                OnInactivated();
        }

        public override ConsoleColor NameBackground => _color;
        public override ConsoleColor NameForeground => ConsoleColor.White;
        public override bool IsActive => Grid.SelectedColor == _color;

        public override IExecutable CreateOperation() => new ColorOperation(Grid, _color);

        private class ColorOperation : IExecutable
        {
            private readonly ConsoleColor _color;
            private readonly Canvas _grid;

            public ColorOperation(Canvas grid, ConsoleColor color)
            {
                _grid = grid;
                _color = color;
            }

            public bool Execute() => _grid.SelectedColor != (_grid.SelectedColor = _color);
        }
    }
}