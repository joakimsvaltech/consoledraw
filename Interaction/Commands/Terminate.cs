using ConsoleDraw.Core;
using ConsoleDraw.Core.Events;
using ConsoleDraw.Interaction.Operations;
using System;

namespace ConsoleDraw.Interaction.Commands
{
    public abstract class Terminate : Command
    {
        protected IApplyable? LastOperation;

        protected Terminate(Canvas grid, ConsoleKey key, string tag, string name)
            : base(grid, key, tag, name)
        {
            grid.CommandExecuted += Grid_CommandExecuted;
        }

        public override bool IsEnabled => LastOperation != null;

        private void Grid_CommandExecuted(object sender, OperationEventArgs e)
        {
            var wasEnabled = IsEnabled;
            LastOperation = e.Operation switch
            {
                IModus _ => LastOperation,
                IApplyable a => a,
                _ => null
            };
            if (wasEnabled != IsEnabled)
                OnStatusChanged();
        }
    }
}