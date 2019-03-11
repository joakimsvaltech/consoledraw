using ConsoleDraw.Core.Events;
using ConsoleDraw.Core.Interaction;
using ConsoleDraw.Interaction.Operations;
using System;

namespace ConsoleDraw.Core
{
    public class Color : Command
    {
        private readonly ConsoleColor _color;
        public bool _isActive;

        public Color(Canvas grid, ConsoleColor color)
            : base(grid, Enum.Parse<ConsoleKey>($"D{(int)color}"), $"{(int)color}", $"{color}")
        {
            grid.ColorChanged += Grid_ColorChanged;
            _color = color;
            _isActive = Grid.SelectedColor == _color;
        }

        private void Grid_ColorChanged(object sender, ColorEventArgs e)
        {
            if (IsActive == (Grid.SelectedColor == _color))
                return;
            _isActive = !_isActive;
            OnStatusChanged();
        }

        public override ConsoleColor? NameBackground => _color;
        public override ConsoleColor? NameForeground => ConsoleColor.White;
        public override bool IsActive => _isActive;

        public override IExecutable CreateOperation() => new SelectColor(Grid, _color);
    }
}