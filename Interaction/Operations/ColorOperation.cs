using ConsoleDraw.Core;
using ConsoleDraw.Core.Interaction;
using System;

namespace ConsoleDraw.Interaction.Operations
{
    public class ColorOperation : IExecutable
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