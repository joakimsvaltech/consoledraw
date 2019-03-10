using ConsoleDraw.Core.Events;
using ConsoleDraw.Core.Interaction;
using ConsoleDraw.Interaction.Operations;
using System;

namespace ConsoleDraw.Core
{
    public partial class ColorCommand : CommandBase
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

        public override ConsoleColor? NameBackground => _color;
        public override ConsoleColor? NameForeground => ConsoleColor.White;
        public override bool IsActive => Grid.SelectedColor == _color;

        public override IExecutable CreateOperation() => new ColorOperation(Grid, _color);
    }
}