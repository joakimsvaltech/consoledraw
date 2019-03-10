using ConsoleDraw.Core.Interaction;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.Core
{
    public class FillCommand : CommandBase
    {
        internal FillCommand(Canvas grid) : base(grid, "_Fill") { }
        public override IExecutable CreateOperation() => new FillOperation(Grid);

        private class FillOperation : PaintOperation
        {
            public FillOperation(Canvas grid) : base(grid) { }

            protected override void Paint()
            {
                Grid.Paint(GetFilledCells());
            }

            protected override Cell[] GetShadow() => GetArea().Select(c => c.Clone()).ToArray();

            private IEnumerable<Cell> GetFilledCells() => GetArea().Select(c => c.Clone(color: Grid.SelectedColor));

            private IEnumerable<Cell> GetArea()
            {
                var next = Grid[Grid.CurrentPos];
                var color = next.Color;
                var area = new HashSet<Cell> { next };
                var neighbours = new Stack<Cell>(next.Neighbours(Grid).Where(n => n.Color == color));
                while (neighbours.Any())
                {
                    next = neighbours.Pop();
                    area.Add(next);
                    next.Neighbours(Grid).Where(n => n.Color == color).Except(area).ForEach(n => neighbours.Push(n));
                }
                return area;
            }
        }
    }
}