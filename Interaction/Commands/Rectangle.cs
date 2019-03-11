using ConsoleDraw.Core;
using ConsoleDraw.Core.Geometry;
using ConsoleDraw.Interaction.Operations;

namespace ConsoleDraw.Interaction.Commands
{
    public class Rectangle : Shape<Core.Geometry.Rectangle>
    {
        internal Rectangle(Canvas grid) : base(grid, "_Rectangle", grid => new Operation(grid)) { }

        public class Operation : Operations.Shape<Core.Geometry.Rectangle>
        {
            public Operation(Canvas grid) : base(grid, pos => new Core.Geometry.Rectangle(pos)) { }
        }
    }
}