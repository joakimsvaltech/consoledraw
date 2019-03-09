using ConsoleDraw.Core.Shapes;

namespace ConsoleDraw.Core
{
    public class EllipseCommand : ShapeCommand<EllipseOperation>
    {
        internal EllipseCommand(Grid grid) : base(grid, "_Ellipse", grid => new EllipseOperation(grid)) { }
    }

    public class EllipseOperation : ShapeOperation<Ellipse>
    {
        public EllipseOperation(Grid grid) : base(grid, pos => new Ellipse(pos)) {}
    }
}