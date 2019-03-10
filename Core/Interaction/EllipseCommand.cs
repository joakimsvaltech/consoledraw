using ConsoleDraw.Core.Geometry;

namespace ConsoleDraw.Core
{
    public class EllipseCommand : ShapeCommand<EllipseOperation>
    {
        internal EllipseCommand(Canvas grid) : base(grid, "_Ellipse", grid => new EllipseOperation(grid)) { }
    }

    public class EllipseOperation : ShapeOperation<Ellipse>
    {
        public EllipseOperation(Canvas grid) : base(grid, pos => new Ellipse(pos)) {}
    }
}