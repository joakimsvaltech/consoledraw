using ConsoleDraw.Core.Commands.Operations;
using ConsoleDraw.Core.Interaction;

namespace ConsoleDraw.Core
{
    public class Plot : CommandBase
    {
        internal Plot(Canvas grid) : base(grid, "_Plot") { }
        public override IExecutable CreateOperation() => new PlotOperation(Grid);

        private class PlotOperation : UndoableOperation
        {
            private Cell? _oldCell;
            private Cell? _newCell;

            public PlotOperation(Canvas grid) : base(grid) { }

            protected override bool DoExecute()
            {
                _oldCell = Grid.CurrentCell.Clone();
                Grid.Plot();
                _newCell = Grid.CurrentCell.Clone();
                return _oldCell! != _newCell!;
            }

            protected override bool DoUndo() => Replot(_newCell, _oldCell);

            protected override bool DoRedo() => Replot(_oldCell, _newCell);

            private bool Replot(Cell? from, Cell? to)
            {
                if (to is null || to == from!)
                    return false;
                Grid.Plot(to);
                return true;
            }
        }
    }
}