using System;

namespace ConsoleDraw.Core
{
    public abstract class ShapeCommand : CommandBase
    {
        private readonly Grid _grid;

        internal ShapeCommand(Grid grid, string label, GridMode shapeMode) : base(label)
            => (_grid, ShapeMode) = (grid, shapeMode);

        protected override ConsoleColor TagBackground => _grid.Mode == ShapeMode
            ? ConsoleColor.White
            : base.TagBackground;

        protected override ConsoleColor TagForeground => _grid.Mode == ShapeMode
            ? ConsoleColor.Black
            : base.TagForeground;

        public GridMode ShapeMode { get; }
    }
}