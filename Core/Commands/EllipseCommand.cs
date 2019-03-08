namespace ConsoleDraw.Core
{
    public class EllipseCommand : ShapeCommand
    {
        internal EllipseCommand(Grid grid) : base(grid, "_Ellipse", GridMode.Ellipse) { }
    }
}