namespace ConsoleDraw.Core
{
    public class RectangleCommand : ShapeCommand
    {
        internal RectangleCommand(Grid grid) : base(grid, "_Rectangle", GridMode.Rectangle) { }
    }
}