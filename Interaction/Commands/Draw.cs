using ConsoleDraw.Core;
using ConsoleDraw.Geometry;

namespace ConsoleDraw.Interaction.Commands
{
    public class Draw : Shape<Doodle>
    {
        internal Draw(Canvas canvas) : base(canvas, "_Draw", canvas => new Operation(canvas)) { }

        private class Operation : Operations.Shape<Doodle>
        {
            public Operation(Canvas grid) : base(grid, pos => new Doodle(pos)) { }
        }
    }
}