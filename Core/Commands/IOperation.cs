namespace ConsoleDraw.Core
{
    public interface IOperation
    {
        void Execute();
        void Undo();
        void Apply();
        bool CanUndo { get; }
        bool CanApply { get; }
    }
}