using ConsoleDraw.Core.Commands.Operations;
using ConsoleDraw.Core.Shapes;
using System.Linq;

namespace ConsoleDraw.Core
{
    public abstract class ShapeOperation : UndoableOperation, IApplyableOperation
    {
        private Cell[] _oldCells = new Cell[0];
        private Cell[] _newCells = new Cell[0];
        private readonly ShapeCommand _command;
        private IShape? _activeShape;

        public ShapeOperation(ShapeCommand command, Grid grid) : base(grid) => _command = command;

        public bool Apply()
        {
            if (_activeShape is null)
                return false;
            ExitShape();
            _oldCells = Grid.GetShadow(_activeShape);
            Grid.FillShape(_activeShape);
            _newCells = Grid.GetShadow(_activeShape);
            Grid.Mode = GridMode.None;
            return _oldCells.SequenceEqual(_newCells);
        }

        protected override bool DoUndo() => Refill(_newCells, _oldCells);

        protected override bool DoRedo() => Refill(_oldCells, _newCells);

        private bool Refill(Cell[] from, Cell[] to)
        {
            if (from.SequenceEqual(to))
                return false;
            to.ForEach(Grid.Plot);
            return true;
        }

        protected override bool DoExecute()
        {
            if (Grid.Mode == _command.ShapeMode)
                Grid.Mode = GridMode.None;
            else
                StartDrawShape();
            return Grid.Mode == _command.ShapeMode;
        }

        protected abstract IShape CreateShape();

        private void StartDrawShape()
        {
            Grid.Mode = _command.ShapeMode;
            Grid.CommandExecuted += Grid_CommandExecuted;
        }

        private void Grid_CommandExecuted(object sender, OperationEventArgs e)
        {
            if (_activeShape == null)
                InitShape();
            else if (e.Operation is NavigationOperation)
                UpdateShape();
            else
                ExitShape();
        }

        private void InitShape()
        {
            _activeShape = CreateShape();
            Grid.Mark(_activeShape);
        }

        private void ExitShape()
        {
            Grid.CommandExecuted -= Grid_CommandExecuted;
            Grid.Unmark(_activeShape);
        }

        private void UpdateShape()
        {
            Grid.Unmark(_activeShape);
            _activeShape!.Update(Grid.CurrentPos);
            Grid.Mark(_activeShape);
        }
    }
}