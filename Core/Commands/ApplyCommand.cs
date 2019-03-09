using ConsoleDraw.Core.Commands.Operations;
using System;

namespace ConsoleDraw.Core
{
    public class ApplyCommand : CommandBase
    {
        private IApplyableOperation? _lastOperation;

        public ApplyCommand(Grid grid)
            : base(ConsoleKey.Enter, "Enter", "Apply")
        {
            grid.CommandExecuted += Grid_CommandExecuted;
        }

        public override IOperation CreateOperation(Grid grid)
            => new ApplyOperation(this, grid);

        private void Grid_CommandExecuted(object sender, OperationEventArgs e)
        {
            if (e.Operation is IApplyableOperation aop)
                _lastOperation = aop;
        }

        private class ApplyOperation : Operation
        {
            private readonly ApplyCommand _command;

            public ApplyOperation(ApplyCommand command, Grid grid) : base(grid) => _command = command;
            public override bool Execute() => _command._lastOperation?.Apply() ?? false;
        }
    }
}