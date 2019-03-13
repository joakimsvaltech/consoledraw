using ConsoleDraw.Core;
using System;
using System.Linq;

namespace ConsoleDraw.Interaction.Operations
{
    public abstract class Paint : Undoable
    {
        private Cell[] _oldCells = new Cell[0];
        private Cell[] _newCells = new Cell[0];

        public Paint(Canvas grid) : base(grid) { }

        protected override bool DoExecute()
        {
            _oldCells = GetShadow();
            Apply();
            _newCells = GetShadow();
            return !_oldCells.SequenceEqual(_newCells);
        }

        protected abstract void Apply();
        protected abstract Cell[] GetShadow();

        protected override bool DoUndo() => Refill(_newCells, _oldCells);

        protected override bool DoRedo() => Refill(_oldCells, _newCells);

        private bool Refill(Cell[] from, Cell[] to)
        {
            if (from.SequenceEqual(to))
                return false;
            Canvas.Paint(to);
            return true;
        }
    }
}