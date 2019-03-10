using ConsoleDraw.Core.Commands.Operations;
using System;
using System.Linq;

namespace ConsoleDraw.Core
{
    public abstract class PaintOperation : UndoableOperation
    {
        private Cell[] _oldCells = new Cell[0];
        private Cell[] _newCells = new Cell[0];

        public PaintOperation(Canvas grid) : base(grid) { }

        protected override bool DoExecute()
        {
            _oldCells = GetShadow();
            Paint();
            _newCells = GetShadow();
            return !_oldCells.SequenceEqual(_newCells);
        }

        protected abstract void Paint();
        protected abstract Cell[] GetShadow();

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