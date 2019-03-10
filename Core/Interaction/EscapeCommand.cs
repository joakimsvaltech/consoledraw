using ConsoleDraw.Core.Commands.Operations;
using System;

namespace ConsoleDraw.Core
{
    public class EscapeCommand : CommandBase
    {
        private IApplyable? _lastOperation;

        public EscapeCommand(Canvas grid) : base(grid, ConsoleKey.Escape, "Esc", "Escape")
        {
            grid.CommandExecuted += Grid_CommandExecuted;
        }

        private void Grid_CommandExecuted(object sender, OperationEventArgs e)
        {
            if (e.Operation is IApplyable aop)
                _lastOperation = aop;
        }

        public override IExecutable CreateOperation() => new EscapeOperation(this);

        private class EscapeOperation : IExecutable
        {
            private readonly EscapeCommand _command;

            public EscapeOperation(EscapeCommand command) => _command = command;
            public bool Execute() => _command._lastOperation?.Deactivate() ?? false;
        }
    }
}