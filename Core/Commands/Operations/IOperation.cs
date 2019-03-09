namespace ConsoleDraw.Core.Commands.Operations
{
    public interface IOperation
    {
        bool Execute();
    }

    public interface IUndoableOperation : IOperation
    {
        bool Undo();
        bool Redo();
    }

    public interface IApplyableOperation : IUndoableOperation
    {
        bool Apply();
    }
}