namespace ConsoleDraw.Core.Commands.Operations
{
    public abstract class UndoableOperation : Operation, IUndoable
    {
        private Point _oldPosition = new Point();
        private Point _newPosition = new Point();
        protected UndoableOperation(Grid grid) : base(grid) { }

        public override bool Execute()
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