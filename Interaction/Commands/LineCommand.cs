using ConsoleDraw.Core.Commands.Operations;
using ConsoleDraw.Core.Geometry;

namespace ConsoleDraw.Core
{
    public class LineCommand : ShapeCommand<LineOperation>
    {
        internal LineCommand(Canvas grid) : base(grid, "_Line", grid => new LineOperation(grid)) { }
    }

    public class LineOperation : ShapeOperation<Line>
    {
        public LineOperation(Canvas grid) : base(grid, pos => new Line(pos))
        {
        }
    }
}