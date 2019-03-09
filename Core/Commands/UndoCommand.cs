using ConsoleDraw.Core.Commands.Operations;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.Core
{
    public partial class UndoCommand : CommandBase
    {
        private readonly Grid _grid;
        private readonly Stack<IUndoable> _operations = new Stack<IUndoable>();

        internal UndoCommand(Grid grid) : base("_Undo") {
            _grid = grid;
            _grid.CommandExecuted += Grid_CommandExecuted;
        }

        internal IUndoable? Pop() => _operations.Any() ? _operations.Pop() : null;

        private void Grid_CommandExecuted(object sender, OperationEventArgs e)
        {
            if (e.Operation is IUndoable uop && !(uop is UndoOperation))
                _operations.Push(uop);
        }

        public override IExecutable CreateOperation(Grid grid) => new UndoOperation(this, grid);
    }
}