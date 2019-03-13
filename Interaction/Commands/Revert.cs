using ConsoleDraw.Core;
using ConsoleDraw.Core.Interaction;
using ConsoleDraw.Interaction.Operations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.Interaction.Commands
{
    public abstract class Revert<TOperation, TRevert> : Command
        where TOperation : class, IUndoable
    {
        private readonly Stack<TOperation> _operations = new Stack<TOperation>();

        protected Revert(Canvas grid, string label) : base(grid, label)
        {
            Init();
        }

        protected Revert(Canvas grid, ConsoleKey key, string tag, string name, ConsoleModifiers modifiers)
            : base(grid, key, tag, name, modifiers)
        {
            Init();
        }

        private void Init()
        {
            Grid.CommandExecuted += Grid_CommandExecuted;
        }

        internal TOperation? Pop()
        {
            var toRevert = _operations.Any() ? _operations.Pop() : null;
            if (!_operations.Any())
                OnStatusChanged();
            return toRevert;
        }

        private void Grid_CommandExecuted(object sender, EventArgs<IExecutable> e)
        {
            var wasEnabled = IsEnabled;
            if (e.Model is TOperation uop && !(uop is TRevert))
                _operations.Push(uop);
            if (wasEnabled != IsEnabled)
                OnStatusChanged();
        }

        public override bool IsEnabled => _operations.Any();
    }
}