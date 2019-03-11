using ConsoleDraw.Core.Interaction;
using ConsoleDraw.Interaction.Commands;
using ConsoleDraw.Interaction.Operations;
using System;

namespace ConsoleDraw.Core
{
    public class Apply : Terminate
    {
        public Apply(Canvas grid) : base(grid, ConsoleKey.Enter, "Enter", "Apply") {}
        public override IExecutable CreateOperation() => new Operation(this, Grid);

        private class Operation : Undoable
        {
            private readonly Apply _command;
            private IApplyable? _appliedOperation;
            public Operation(Apply command, Canvas grid) : base(grid) => _command = command;
            protected override bool DoExecute() => (_appliedOperation = _command.LastOperation)?.Apply() ?? false;
            protected override bool DoRedo() => _appliedOperation?.Reapply() ?? false;
            protected override bool DoUndo() => _appliedOperation?.Unapply() ?? false;
        }
    }
}