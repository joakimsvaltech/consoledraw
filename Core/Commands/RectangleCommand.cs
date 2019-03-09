using ConsoleDraw.Core.Commands.Operations;
using ConsoleDraw.Core.Shapes;

namespace ConsoleDraw.Core
{
    public class RectangleCommand : ShapeCommand<RectangleOperation>
    {
        internal RectangleCommand(Grid grid) : base(grid, "_Rectangle", grid => new RectangleOperation(grid)) { }
    }

    public class RectangleOperation : ShapeOperation<Rectangle>
    {
        public RectangleOperation(Grid grid) : base(grid, pos => new Rectangle(pos)) {}
    }
}