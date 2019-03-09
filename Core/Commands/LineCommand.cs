using ConsoleDraw.Core.Commands.Operations;
using ConsoleDraw.Core.Shapes;

namespace ConsoleDraw.Core
{
    public class LineCommand : ShapeCommand
    {
        internal LineCommand(Grid grid) : base(grid, "_Line", GridMode.Line) { }
        public override IOperation CreateOperation(Grid grid) => new LineOperation(this, grid);

        private class LineOperation : ShapeOperation
        {
            public LineOperation(ShapeCommand command, Grid grid) : base(command, grid)
            {
            }

            protected override IShape CreateShape() => new Line(Grid.CurrentPos);
        }
    }
}