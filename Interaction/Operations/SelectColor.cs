using ConsoleDraw.Core;
using ConsoleDraw.Core.Interaction;
using System;

namespace ConsoleDraw.Interaction.Operations
{
    public class SelectColor : IExecutable, IModus
    {
        private readonly ConsoleColor _color;
        private readonly Canvas _grid;

        public SelectColor(Canvas grid, ConsoleColor color)
        {
            _grid = grid;
            _color = color;
        }

        public bool Execute() => _grid.SelectedColor != (_grid.SelectedColor = _color);
    }
}