using ConsoleDraw.Core;

namespace ConsoleDraw.Interaction.Operations
{
    public class Undo : Undoable
    {
        private readonly Commands.Undo _command;
        private IUndoable? _undoneOperation;
        public Undo(Commands.Undo command, Canvas grid) : base(grid) => _command = command;

        protected override bool DoExecute()
            => (_undoneOperation = _command.Pop())?.Undo() ?? false;

        protected override bool DoRedo() => _undoneOperation?.Undo() ?? false;

        protected override bool DoUndo() => _undoneOperation?.Redo() ?? false;
    }
}