namespace ConsoleDraw.Interaction.Operations
{
    public interface IUndoable
    {
        bool Undo();
        bool Redo();
    }
}