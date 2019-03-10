namespace ConsoleDraw.Core.Commands.Operations
{
    public interface IUndoable
    {
        bool Undo();
        bool Redo();
    }
}