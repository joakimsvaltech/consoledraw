using ConsoleDraw.Core;

namespace ConsoleDraw.Interaction.Commands
{
    public class Ellipse : Shape<Geometry.Ellipse>
    {
        internal Ellipse(Canvas grid) : base(grid, "_Ellipse", grid => new Operation(grid)) { }

        private class Operation : Operations.Shape<Geometry.Ellipse>
        {
            public Operation(Canvas grid) : base(grid, pos => new Geometry.Ellipse(pos)) { }
        }
    }
}