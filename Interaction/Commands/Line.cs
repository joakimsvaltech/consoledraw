using ConsoleDraw.Core;

namespace ConsoleDraw.Interaction.Commands
{
    public class Line : Shape<Geometry.Line>
    {
        internal Line(Canvas grid) : base(grid, "_Line", grid => new Operation(grid)) { }

        private class Operation : Operations.Shape<Geometry.Line>
        {
            public Operation(Canvas grid) : base(grid, pos => new Geometry.Line(pos))
            {
            }
        }
    }
}