namespace ConsoleDraw.Core.Commands.Operations
{
    public class UndoOperation : UndoableOperation
    {
        private readonly UndoCommand _command;
        private IUndoableOperation? _undoneOperation;
        public UndoOperation(UndoCommand command, Grid grid) : base(grid) => _command = command;

        protected override bool DoExecute()
            => (_undoneOperation = _command.Pop())?.Undo() ?? false;

        protected override bool DoRedo() => _undoneOperation?.Undo() ?? false;

        protected override bool DoUndo() => _undoneOperation?.Redo() ?? false;
    }
}