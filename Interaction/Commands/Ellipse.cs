using ConsoleDraw.Core;
namespace ConsoleDraw.Interaction.Commands
{
    public class Ellipse : Shape<Core.Geometry.Ellipse>
    {
        internal Ellipse(Canvas grid) : base(grid, "_Ellipse", grid => new Operation(grid)) { }

        private class Operation : Operations.Shape<Core.Geometry.Ellipse>
        {
            public Operation(Canvas grid) : base(grid, pos => new Core.Geometry.Ellipse(pos)) { }
        }
    }
}