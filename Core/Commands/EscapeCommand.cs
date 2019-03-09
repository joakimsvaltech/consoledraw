using ConsoleDraw.Core.Commands.Operations;
using System;

namespace ConsoleDraw.Core
{
    public class EscapeCommand : CommandBase
    {
        private IApplyable? _lastOperation;

        public EscapeCommand(Grid grid) : base(ConsoleKey.Escape, "Esc", "Escape")
        {
            grid.CommandExecuted += Grid_CommandExecuted;
        }

        private void Grid_CommandExecuted(object sender, OperationEventArgs e)
        {
            if (e.Operation is IApplyable aop)
                _lastOperation = aop;
        }

        public override IExecutable CreateOperation(Grid grid) => new EscapeOperation(this, grid);

        private class EscapeOperation : Operation
        {
            private readonly EscapeCommand _command;

            public EscapeOperation(EscapeCommand command, Grid grid) : base(grid) => _command = command;
            public override bool Execute() => _command._lastOperation?.Deactivate() ?? false;
        }
    }
}