using ConsoleDraw.Core;
using ConsoleDraw.Core.Geometry;
using ConsoleDraw.Core.Interaction;

namespace ConsoleDraw.Interaction.Operations
{
    public abstract class Undoable : IExecutable, IUndoable
    {
        private Point _oldPosition;
        private Point _newPosition;
        protected readonly Canvas Canvas;

        protected Undoable(Canvas grid) => Canvas = grid;

        public bool Execute()
        {
            _oldPosition = Canvas.CurrentPos;
            return DoExecute();
        }

        public bool Undo()
        {
            _newPosition = Canvas.CurrentPos;
            var res = DoUndo();
            Canvas.CurrentPos = _oldPosition;
            return res || _newPosition != _oldPosition;
        }

        public bool Redo()
        {
            var res = DoRedo();
            Canvas.CurrentPos = _newPosition;
            return res || _newPosition != _oldPosition;
        }

        protected abstract bool DoExecute();
        protected abstract bool DoUndo();
        protected abstract bool DoRedo();
    }
}