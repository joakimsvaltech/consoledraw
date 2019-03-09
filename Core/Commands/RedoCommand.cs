using ConsoleDraw.Core.Commands.Operations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.Core
{
    public class RedoCommand : CommandBase
    {
        private readonly Stack<UndoOperation> _undos = new Stack<UndoOperation>();

        internal RedoCommand(Grid grid) : base(ConsoleKey.U, "Shift-U", "Redo", ConsoleModifiers.Shift)
        {
            grid.CommandExecuted += Grid_CommandExecuted;
        }

        private void Grid_CommandExecuted(object sender, OperationEventArgs e)
        {
            if (e.Operation is UndoOperation uop)
                _undos.Push(uop);
        }

        public override IOperation CreateOperation(Grid grid) => new RedoOperation(this, grid);

        internal UndoOperation? Pop() => _undos.Any() ? _undos.Pop() : null;

        private class RedoOperation : UndoableOperation
        {
            private readonly RedoCommand _command;
            private UndoOperation? _undone;
            public RedoOperation(RedoCommand command, Grid grid) : base(grid) => _command = command;

            protected override bool DoExecute() => (_undone = _command.Pop())?.Undo() ?? false;

            protected override bool DoRedo() => _undone?.Undo() ?? false;

            protected override bool DoUndo()  => _undone?.Redo() ?? false;
        }
    }
}