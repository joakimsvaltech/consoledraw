using ConsoleDraw.Core;
using ConsoleDraw.Core.Interaction;
using ConsoleDraw.Interaction.Operations;
using System;

namespace ConsoleDraw.Interaction.Commands
{
    public class Redo : Revert<Operations.Undo, Redo>
    {
        internal Redo(Canvas grid) : base(grid, ConsoleKey.U, "Shift-U", "Redo", ConsoleModifiers.Shift)
        {
        }

        public override IExecutable CreateOperation() => new Operation(this, Grid);

        private class Operation : Undoable
        {
            private readonly Redo _command;
            private Operations.Undo? _undone;
            public Operation(Redo command, Canvas grid) : base(grid) => _command = command;

            protected override bool DoExecute() => (_undone = _command.Pop())?.Undo() ?? false;

            protected override bool DoRedo() => _undone?.Undo() ?? false;

            protected override bool DoUndo() => _undone?.Redo() ?? false;
        }
    }
}