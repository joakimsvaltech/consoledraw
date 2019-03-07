using System;

namespace ConsoleDraw.Core
{
    public class Plot : CommandBase
    {
        private readonly Grid _grid;
        internal Plot(Grid grid) : base("_Plot") => _grid = grid;
        public override IOperation CreateOperation() => new PlotOperation(_grid);

        private class PlotOperation : IOperation
        {
            private readonly Grid _grid;
            private Point _oldPos;
            private ConsoleColor _oldColor;

            public PlotOperation(Grid grid) => _grid = grid;

            public bool CanUndo => true;

            public void Execute()
            {
                var cell = _grid.Peek();
                _oldPos = cell.Pos;
                _oldColor = cell.Color;
                _grid.Plot();
            }

            public void Undo()
            {
                _grid.Plot(_oldPos, _oldColor);
            }
        }
    }
}