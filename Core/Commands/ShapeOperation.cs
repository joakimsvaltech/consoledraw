﻿using ConsoleDraw.Core.Shapes;

namespace ConsoleDraw.Core
{
    public abstract class ShapeOperation : ApplyableOperation
    {
        private Cell[] _oldCells = new Cell[0];
        private readonly ShapeCommand _command;
        private IShape? _activeShape;

        public ShapeOperation(ShapeCommand command, Grid grid) : base(grid) => _command = command;

        public override void Apply()
        {
            if (_activeShape is null)
                return;
            _oldCells = Grid.GetShadow(_activeShape);
            Grid.FillShape(_activeShape);
            Grid.Mode = GridMode.None;
        }

        protected override void DoUndo()
        {
            _oldCells.ForEach(Grid.Plot);
        }

        protected override void DoExecute()
        {
            if (Grid.Mode == _command.ShapeMode)
                Grid.Mode = GridMode.None;
            else
                StartDrawShape();
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