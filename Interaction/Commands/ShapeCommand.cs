using ConsoleDraw.Core.Events;
using ConsoleDraw.Core.Interaction;
using System;

namespace ConsoleDraw.Core
{
    public abstract class ShapeCommand<TShapeOperation> : CommandBase
        where TShapeOperation : class, IShapeOperation
    {
        private TShapeOperation? _activeOperation;
        private readonly Func<Canvas, TShapeOperation> _create;

        internal ShapeCommand(Canvas grid, string label, Func<Canvas, TShapeOperation> create) : base(grid, label)
        {
            _create = create;
            grid.CommandExecuting += Grid_CommandExecuting;
        }

        public override IExecutable CreateOperation() => _create(Grid);

        private void Grid_CommandExecuting(object sender, OperationEventArgs e)
        {
            bool wasActive = IsActive;
            if (e.Operation is TShapeOperation sop)
            {
                _activeOperation = sop;
                _activeOperation.Deactivated += ActiveOperation_Deactivated;
                if (!wasActive) OnActivated();
            }
        }

        private void ActiveOperation_Deactivated(object sender, EventArgs e)
        {
            if (_activeOperation == sender)
            {
                _activeOperation = null;
                OnInactivated();
            }
        }

        public override bool IsActive => !(_activeOperation is null);
    }
}