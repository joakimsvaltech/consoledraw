using ConsoleDraw.Core.Commands.Operations;
using ConsoleDraw.Core.Geometry;

namespace ConsoleDraw.Core
{
    public class RectangleCommand : ShapeCommand<RectangleOperation>
    {
        internal RectangleCommand(Canvas grid) : base(grid, "_Rectangle", grid => new RectangleOperation(grid)) { }
    }

    public class RectangleOperation : ShapeOperation<Rectangle>
    {
        public RectangleOperation(Canvas grid) : base(grid, pos => new Rectangle(pos)) {}
    }
}