using ConsoleDraw.Core.Commands.Operations;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.Core
{
    public partial class UndoCommand : CommandBase
    {
        private readonly Grid _grid;
        private readonly Stack<IOperation> _operations = new Stack<IOperation>();

        internal UndoCommand(Grid grid) : base("_Undo") {
            _grid = grid;
            _grid.CommandExecuted += Grid_CommandExecuted;
        }

        internal IOperation? Pop() => _operations.Any() ? _operations.Pop() : null;

        private void Grid_CommandExecuted(object sender, OperationEventArgs e)
        {
            if (e.Operation.CanUndo && !(e.Operation is UndoOperation))
                _operations.Push(e.Operation);
        }

        public override IOperation CreateOperation(Grid grid) => new UndoOperation(this, grid);
    }
}