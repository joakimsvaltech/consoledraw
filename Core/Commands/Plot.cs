using System;

namespace ConsoleDraw.Core
{
    public class Plot : CommandBase
    {
        internal Plot() : base("_Plot") { }
        public override IOperation CreateOperation(Grid grid) => new PlotOperation(grid);

        private class PlotOperation : UndoableOperation
        {
            private Point _oldPos;
            private ConsoleColor _oldColor;

            public PlotOperation(Grid grid) : base(grid) { }

            protected override void DoExecute()
            {
                var cell = Grid.Peek();
                _oldPos = cell.Pos;
                _oldColor = cell.Color;
                Grid.Plot();
            }

            protected override void DoUndo()
            {
                Grid.Plot(_oldPos, _oldColor);
            }
        }
    }
}