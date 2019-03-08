using ConsoleDraw.Core.Shapes;

namespace ConsoleDraw.Core
{
    public class EllipseCommand : ShapeCommand
    {
        internal EllipseCommand(Grid grid) : base(grid, "_Ellipse", GridMode.Ellipse) { }
        public override IOperation CreateOperation(Grid grid) => new EllipseOperation(this, grid);

        private class EllipseOperation : ShapeOperation
        {
            public EllipseOperation(ShapeCommand command, Grid grid) : base(command, grid)
            {
            }

            protected override IShape CreateShape() => new Ellipse(Grid.CurrentPos);
        }
    }
}