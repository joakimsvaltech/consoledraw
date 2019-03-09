using ConsoleDraw.Core.Commands.Operations;
using System;

namespace ConsoleDraw.Core
{
    public abstract class ShapeCommand<TShapeOperation> : CommandBase
        where TShapeOperation : class, IShapeOperation
    {
        private TShapeOperation? _activeOperation;
        private readonly Func<Grid, TShapeOperation> _create;

        internal ShapeCommand(Grid grid, string label, Func<Grid, TShapeOperation> create) : base(label)
        {
            grid.CommandExecuting += Grid_CommandExecuting;
            _create = create;
        }

        public override IExecutable CreateOperation(Grid grid) => _create(grid);

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