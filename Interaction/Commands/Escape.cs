using ConsoleDraw.Core.Interaction;
using ConsoleDraw.Interaction.Commands;
using System;

namespace ConsoleDraw.Core
{
    public class Escape : Terminate
    {
        public Escape(Canvas grid) : base(grid, ConsoleKey.Escape, "Esc", "Escape") {}
        public override IExecutable CreateOperation() => new Operation(this);

        private class Operation : IExecutable
        {
            private readonly Escape _command;
            public Operation(Escape command) => _command = command;
            public bool Execute() => _command.LastOperation?.Deactivate() ?? false;
        }
    }
}