using ConsoleDraw.Core.Commands.Operations;
using System.Linq;

namespace ConsoleDraw.Core
{
    public class FillCommand : CommandBase
    {
        internal FillCommand(Grid grid) : base(grid, "_Fill") { }
        public override IExecutable CreateOperation() => new FillOperation(Grid);

        private class FillOperation : UndoableOperation
        {
            private Cell[] _oldCells = new Cell[0];
            private Cell[] _newCells = new Cell[0];

            public FillOperation(Grid grid) : base(grid) { }

            protected override bool DoExecute()
            {
                _oldCells = GetFillShadow();
                Grid.Fill();
                _newCells = GetFillShadow();
                return !_oldCells.SequenceEqual(_newCells);
            }

            private Cell[] GetFillShadow() => Grid.GetArea(Grid.CurrentPos).Select(c => c.Clone()).ToArray();

            protected override bool DoUndo() => Refill(_newCells, _oldCells);

            protected override bool DoRedo() => Refill(_oldCells, _newCells);

            private bool Refill(Cell[] from, Cell[] to)
            {
                if (from.SequenceEqual(to))
                    return false;
                to.ForEach(Grid.Plot);
                return true;
            }
        }
    }
}