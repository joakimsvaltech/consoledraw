namespace ConsoleDraw.Core
{
    public abstract class ShapeCommand : CommandBase
    {
        private readonly Grid _grid;
        internal ShapeCommand(Grid grid, string label, GridMode shapeMode) : base(label)
            => (_grid, ShapeMode) = (grid, shapeMode);
        protected override bool IsActive => _grid.Mode == ShapeMode;
        public GridMode ShapeMode { get; }
    }
}