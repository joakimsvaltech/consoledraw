using ConsoleDraw.Core.Interaction;
using ConsoleDraw.Interaction.Operations;

namespace ConsoleDraw.Core
{
    public class Plot : Command
    {
        internal Plot(Canvas grid) : base(grid, "_Plot") { }
        public override IExecutable CreateOperation() => new Operation(Grid);

        private class Operation : Undoable
        {
            private Cell? _oldCell;
            private Cell? _newCell;

            public Operation(Canvas grid) : base(grid) { }

            protected override bool DoExecute()
            {
                _oldCell = Canvas.CurrentCell.Clone();
                Canvas.Plot();
                _newCell = Canvas.CurrentCell.Clone();
                return _oldCell! != _newCell!;
            }

            protected override bool DoUndo() => Replot(_newCell, _oldCell);

            protected override bool DoRedo() => Replot(_oldCell, _newCell);

            private bool Replot(Cell? from, Cell? to)
            {
                if (to is null || to == from!)
                    return false;
                Canvas.Plot(to);
                return true;
            }
        }
    }
}