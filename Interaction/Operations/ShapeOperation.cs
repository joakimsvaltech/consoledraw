using ConsoleDraw.Core.Commands.Operations;
using ConsoleDraw.Core.Events;
using ConsoleDraw.Core.Geometry;
using ConsoleDraw.Core.Interaction;
using ConsoleDraw.Interaction.Operations;
using System;
using System.Linq;

namespace ConsoleDraw.Core
{
    public interface IShapeOperation : IExecutable, IApplyable { }

    public abstract class ShapeOperation<TShape> : IShapeOperation
        where TShape : class, IShape
    {
        public event EventHandler<EventArgs> Deactivated;

        private Cell[] _oldCells = new Cell[0];
        private Cell[] _newCells = new Cell[0];
        private TShape? _activeShape;
        private readonly Canvas _grid;
        private readonly Func<Point, TShape> _create;

        public ShapeOperation(Canvas grid, Func<Point, TShape> create)
        {
            _grid = grid;
            _create = create;
        }

        public bool Apply()
        {
            if (_activeShape is null)
                return false;
            ExitShape();
            _oldCells = GetShadow(_activeShape);
            _grid.FillShape(_activeShape);
            _newCells = GetShadow(_activeShape);
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

        private Cell[] GetShadow(IShape shape) => shape.Area.Select(p => _grid[p]).Select(c => c.Clone()).ToArray();

        private bool Refill(Cell[] from, Cell[] to)
        {
            if (from.SequenceEqual(to))
                return false;
            to.ForEach(_grid.Plot);
            return true;
        }

        public bool Execute()
        {
            _grid.CommandExecuted += Grid_CommandExecuted;
            return true;
        }

        private void Grid_CommandExecuted(object sender, OperationEventArgs e)
        {
            if (_activeShape == null)
                InitShape();
            else if (e.Operation is NavigationOperation)
                UpdateShape();
            else if (!(e.Operation is ColorOperation))
                ExitShape();
        }

        private void InitShape()
        {
            _activeShape = _create(_grid.CurrentPos);
            _grid.Highlight.Area = _activeShape.Outline;
        }

        private void ExitShape()
        {
            Deactivated?.Invoke(this, new EventArgs());
            _grid.CommandExecuted -= Grid_CommandExecuted;
            _grid.Highlight.Area = new Point[0];
        }

        private void UpdateShape()
        {
            _activeShape!.Update(_grid.CurrentPos);
            _grid.Highlight.Area = _activeShape.Outline;
        }
    }
}