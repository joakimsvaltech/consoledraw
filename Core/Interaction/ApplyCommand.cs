using ConsoleDraw.Core.Commands.Operations;
using System;

namespace ConsoleDraw.Core
{
    public class ApplyCommand : CommandBase
    {
        private IApplyable? _lastOperation;

        public ApplyCommand(Canvas grid)
            : base(grid, ConsoleKey.Enter, "Enter", "Apply")
        {
            grid.CommandExecuted += Grid_CommandExecuted;
        }

        public override IExecutable CreateOperation()
            => new ApplyOperation(this, Grid);

        private void Grid_CommandExecuted(object sender, OperationEventArgs e)
        {
            if (e.Operation is IApplyable aop)
                _lastOperation = aop;
        }

        private class ApplyOperation : UndoableOperation
        {
            private readonly ApplyCommand _command;
            private IApplyable? _appliedOperation;

            public ApplyOperation(ApplyCommand command, Canvas grid) : base(grid) => _command = command;

            protected override bool DoExecute() => (_appliedOperation = _command._lastOperation)?.Apply() ?? false;

            protected override bool DoRedo() => _appliedOperation?.Reapply() ?? false;

            protected override bool DoUndo() => _appliedOperation?.Unapply() ?? false;
        }
    }
}