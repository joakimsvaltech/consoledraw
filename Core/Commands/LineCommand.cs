using ConsoleDraw.Core.Commands.Operations;
using ConsoleDraw.Core.Shapes;

namespace ConsoleDraw.Core
{
    public class LineCommand : ShapeCommand<LineOperation>
    {
        internal LineCommand(Grid grid) : base(grid, "_Line", grid => new LineOperation(grid)) { }
    }

    public class LineOperation : ShapeOperation<Line>
    {
        public LineOperation(Grid grid) : base(grid, pos => new Line(pos))
        {
        }
    }
}