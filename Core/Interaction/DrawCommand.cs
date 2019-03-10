using ConsoleDraw.Core.Geometry;

namespace ConsoleDraw.Core
{
    public class DrawCommand : ShapeCommand<DrawOperation>
    {
        internal DrawCommand(Canvas grid) : base(grid, "_Draw", rgid => new DrawOperation(grid)) { }
    }

    public class DrawOperation : ShapeOperation<Doodle>
    {
        public DrawOperation(Canvas grid) : base(grid, pos => new Doodle(pos)) { }
    }
}