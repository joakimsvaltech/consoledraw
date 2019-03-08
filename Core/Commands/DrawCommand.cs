using System;
using System.Collections.Generic;

namespace ConsoleDraw.Core
{
    public class DrawCommand : CommandBase
    {
        private readonly Grid _grid;

        internal DrawCommand(Grid grid)
            : base("_Draw") => _grid = grid;

        protected override ConsoleColor TagBackground => _grid.IsDrawing
            ? ConsoleColor.White
            : base.TagBackground;

        protected override ConsoleColor TagForeground => _grid.IsDrawing
            ? ConsoleColor.Black
            : base.TagForeground;


        public override IOperation CreateOperation(Grid grid) => new DrawOperation(grid);

        private class DrawOperation : UndoableOperation
        {
            private readonly List<Cell> _cellsBeforeDraw = new List<Cell>();

            public DrawOperation(Grid grid) : base(grid) { }

            public override bool CanUndo => Grid.IsDrawing;

            protected override void DoExecute()
            {
                if (Grid.Mode != GridMode.Drawing)
                {
                    AddCell(Grid.CurrentPos);
                    Grid.CommandExecuting += Grid_CommandExecuting;
                }
                Grid.ToggleDraw();
            }

            private void Grid_CommandExecuting(object sender, OperationEventArgs e)
            {
                if (e.Operation is NavigationOperation navOp)
                    AddCell(Grid.NextPosition(navOp.Direction));
                else
                    Grid.CommandExecuting -= Grid_CommandExecuting;
            }

            private void AddCell(Point pos) => _cellsBeforeDraw.Add(Grid[pos].Clone());

            protected override void DoUndo()
            {
                _cellsBeforeDraw.ForEach(c => Grid.Plot(c.Pos, c.Color));
                Grid.Mode = GridMode.None;
            }
        }
    }
}