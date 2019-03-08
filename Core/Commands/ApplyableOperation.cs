namespace ConsoleDraw.Core
{
    public abstract class ApplyableOperation : UndoableOperation
    {
        protected ApplyableOperation(Grid grid) : base(grid) { }
        public override bool CanApply => true;
    }
}