using ConsoleDraw.Core;
using ConsoleDraw.Core.Geometry;
using ConsoleDraw.Core.Interaction;
using ConsoleDraw.Interaction.Operations;
using System;

namespace ConsoleDraw.Interaction.Commands
{
    public abstract class Shape<TShape> : Command
        where TShape : class, IShape
    {
        private IApplyable<TShape>? _activeOperation;
        private readonly Func<Canvas, IApplyable<TShape>> _create;

        internal Shape(Canvas grid, string label, Func<Canvas, IApplyable<TShape>> create) : base(grid, label)
        {
            _create = create;
            grid.CommandExecuting += Grid_CommandExecuting;
        }

        public override IExecutable CreateOperation() => _create(Grid);

        private void Grid_CommandExecuting(object sender, EventArgs<IExecutable> e)
        {
            bool wasActive = IsActive;
            if (e.Model is IApplyable<TShape> sop)
            {
                _activeOperation = sop;
                _activeOperation.Deactivated += ActiveOperation_Deactivated;
                if (!wasActive) OnStatusChanged();
            }
        }

        private void ActiveOperation_Deactivated(object sender, EventArgs e)
        {
            if (_activeOperation == sender)
            {
                _activeOperation = null;
                OnStatusChanged();
            }
        }

        public override bool IsActive => !(_activeOperation is null);
    }
}