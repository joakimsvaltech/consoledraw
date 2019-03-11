using ConsoleDraw.Core;
using ConsoleDraw.Interaction.Operations;

namespace ConsoleDraw.Interaction.Commands
{
    public class Rectangle : Shape<Geometry.Rectangle>
    {
        internal Rectangle(Canvas grid) : base(grid, "_Rectangle", grid => new Operation(grid)) { }

        public class Operation : Operations.Shape<Geometry.Rectangle>
        {
            public Operation(Canvas grid) : base(grid, pos => new Geometry.Rectangle(pos)) { }
        }
    }
}