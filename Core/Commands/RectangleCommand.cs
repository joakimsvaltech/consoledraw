using ConsoleDraw.Core.Commands.Operations;
using ConsoleDraw.Core.Shapes;

namespace ConsoleDraw.Core
{
    public class RectangleCommand : ShapeCommand
    {
        internal RectangleCommand(Grid grid) : base(grid, "_Rectangle", GridMode.Rectangle) { }
        public override IOperation CreateOperation(Grid grid) => new RectangleOperation(this, grid);

        private class RectangleOperation : ShapeOperation
        {
            public RectangleOperation(ShapeCommand command, Grid grid) : base(command, grid)
            {
            }

            protected override IShape CreateShape() => new Rectangle(Grid.CurrentPos);
        }
    }
}