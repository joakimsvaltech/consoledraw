namespace ConsoleDraw.Core
{
    public abstract class UndoableOperation : Operation
    {
        private Point _oldPosition = new Point();
        protected UndoableOperation(Grid grid) : base(grid) { }
        public override bool CanUndo => true;
        public override void Execute() {
            _oldPosition = Grid.CurrentPos;
            DoExecute();
        }
        public override void Undo() {
            DoUndo();
            Grid.CurrentPos = _oldPosition;
            Grid.Mode = GridMode.None;
        }
        protected abstract void DoExecute();
        protected abstract void DoUndo();
    }
}