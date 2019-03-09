namespace ConsoleDraw.Core.Commands.Operations
{
    public abstract class ApplyableOperation : UndoableOperation
    {
        protected ApplyableOperation(Grid grid) : base(grid) { }
        public override bool CanApply => true;
    }
}