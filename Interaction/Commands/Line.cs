using ConsoleDraw.Core;

namespace ConsoleDraw.Interaction.Commands
{
    public class Line : Shape<Core.Geometry.Line>
    {
        internal Line(Canvas grid) : base(grid, "_Line", grid => new Operation(grid)) { }

        private class Operation : Operations.Shape<Core.Geometry.Line>
        {
            public Operation(Canvas grid) : base(grid, pos => new Core.Geometry.Line(pos))
            {
            }
        }
    }
}