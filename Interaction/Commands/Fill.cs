using ConsoleDraw.Core.Interaction;
using ConsoleDraw.Interaction.Operations;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.Core
{
    public class Fill : Command
    {
        internal Fill(Canvas grid) : base(grid, "_Fill") { }
        public override IExecutable CreateOperation() => new Operation(Grid);

        private class Operation : Paint
        {
            public Operation(Canvas grid) : base(grid) { }

            protected override void Apply()
            {
                Grid.Paint(GetFilledCells());
            }

            protected override Cell[] GetShadow() => GetArea().Select(c => c.Clone()).ToArray();

            private IEnumerable<Cell> GetFilledCells() => GetArea().Select(c => c.Clone(bg: Grid.SelectedColor));

            private IEnumerable<Cell> GetArea()
            {
                var next = Grid[Grid.CurrentPos];
                var color = next.Brush.Background;
                var area = new HashSet<Cell> { next };
                var neighbours = new Stack<Cell>(next.Neighbours(Grid).Where(n => n.Brush.Background == color));
                while (neighbours.Any())
                {
                    next = neighbours.Pop();
                    area.Add(next);
                    next.Neighbours(Grid).Where(n => n.Brush.Background == color).Except(area).ForEach(n => neighbours.Push(n));
                }
                return area;
            }
        }
    }
}