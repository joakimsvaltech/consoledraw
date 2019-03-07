namespace ConsoleDraw.Core
{
    public interface IOperation
    {
        void Execute();
        void Undo();
        bool CanUndo { get; }
    }
}