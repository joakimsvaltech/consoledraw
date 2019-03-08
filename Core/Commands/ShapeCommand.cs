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
        public override IOperation CreateOperation(Grid grid) => new ShapeOperation(this, grid);
    }

    public class ShapeOperation : ApplyableOperation
    {
        private Cell[] _oldCells = new Cell[0];
        private readonly ShapeCommand _command;

        public ShapeOperation(ShapeCommand command, Grid grid) : base(grid) => _command = command;

        protected override void DoExecute()
        {
            Grid.Mode = _command.ShapeMode;
        }

        public override void Apply()
        {
            var shadow = Grid.ActiveShadow;
            if (shadow == null)
                return;
            _oldCells = shadow;
            Grid.FillShape();
            Grid.Mode = GridMode.None;
        }

        protected override void DoUndo()
        {
            _oldCells.ForEach(Grid.Plot);
        }
    }
}