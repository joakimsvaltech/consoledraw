using ConsoleDraw.Core.Commands.Operations;
using ConsoleDraw.Core.Shapes;
using System;
using System.Linq;

namespace ConsoleDraw.Core
{
    public interface IShapeOperation : IExecutable, IApplyable { }

    public abstract class ShapeOperation<TShape> : Operation, IShapeOperation
        where TShape : class, IShape
    {
        public event EventHandler<EventArgs> Deactivated;

        private Cell[] _oldCells = new Cell[0];
        private Cell[] _newCells = new Cell[0];
        private TShape? _activeShape;
        private readonly Func<Point, TShape> _create;

        public ShapeOperation(Grid grid, Func<Point, TShape> create) : base(grid) => _create = create;

        public bool Apply()
        {
            if (_activeShape is null)
                return false;
            ExitShape();
            _oldCells = Grid.GetShadow(_activeShape);
            Grid.FillShape(_activeShape);
            _newCells = Grid.GetShadow(_activeShape);
            return !_oldCells.SequenceEqual(_newCells);
        }

        public bool Deactivate()
        {
            if (_activeShape is null)
                return false;
            ExitShape();
            return false;
        }

        public bool Reapply() => Refill(_oldCells, _newCells);

        public bool Unapply() => Refill(_newCells, _oldCells);

        private bool Refill(Cell[] from, Cell[] to)
        {
            if (from.SequenceEqual(to))
                return false;
            to.ForEach(Grid.Plot);
            return true;
        }

        public override bool Execute()
        {
            Grid.CommandExecuted += Grid_CommandExecuted;
            return true;
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
            _activeShape = _create(Grid.CurrentPos);
            Grid.Mark(_activeShape);
        }

        private void ExitShape()
        {
            Deactivated?.Invoke(this, new EventArgs());
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