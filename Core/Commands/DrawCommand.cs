using ConsoleDraw.Core.Shapes;

namespace ConsoleDraw.Core
{
    public class DrawCommand : ShapeCommand<DrawOperation>
    {
        internal DrawCommand(Grid grid) : base(grid, "_Draw", rgid => new DrawOperation(grid)) { }
    }

    public class DrawOperation : ShapeOperation<Doodle>
    {
        public DrawOperation(Grid grid) : base(grid, pos => new Doodle(pos)) { }
    }
}