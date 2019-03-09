using ConsoleDraw.Core.Commands.Operations;
using ConsoleDraw.Core.Shapes;

namespace ConsoleDraw.Core
{
    public class DrawCommand : ShapeCommand
    {
        internal DrawCommand(Grid grid) : base(grid, "_Draw", GridMode.Drawing) { }
        public override IOperation CreateOperation(Grid grid) => new DrawOperation(this, grid);

        private class DrawOperation : ShapeOperation
        {
            public DrawOperation(ShapeCommand command, Grid grid) : base(command, grid)
            {
            }

            protected override IShape CreateShape() => new Doodle(Grid.CurrentPos);
        }
    }
}