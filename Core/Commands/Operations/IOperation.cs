namespace ConsoleDraw.Core.Commands.Operations
{
    public interface IOperation
    {
        bool Execute();
        bool Undo();
        bool Redo();
        bool Apply();
        bool CanUndo { get; }
        bool CanApply { get; }
    }
}