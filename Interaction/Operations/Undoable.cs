using ConsoleDraw.Core;
using ConsoleDraw.Core.Geometry;
using ConsoleDraw.Core.Interaction;

namespace ConsoleDraw.Interaction.Operations
{
    public abstract class Undoable : IExecutable, IUndoable
    {
        private Point _oldPosition;
        private Point _newPosition;
        protected readonly Canvas Grid;

        protected Undoable(Canvas grid) => Grid = grid;

        public bool Execute()
        {
            _oldPosition = Grid.CurrentPos;
            return DoExecute();
        }

        public bool Undo()
        {
            _newPosition = Grid.CurrentPos;
            var res = DoUndo();
            Grid.CurrentPos = _oldPosition;
            return res || _newPosition != _oldPosition;
        }

        public bool Redo()
        {
            var res = DoRedo();
            Grid.CurrentPos = _newPosition;
            return res || _newPosition != _oldPosition;
        }

        protected abstract bool DoExecute();
        protected abstract bool DoUndo();
        protected abstract bool DoRedo();
    }
}