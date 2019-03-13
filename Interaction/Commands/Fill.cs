using ConsoleDraw.Core.Geometry;
using ConsoleDraw.Core.Interaction;
using ConsoleDraw.Geometry;
using ConsoleDraw.Interaction.Operations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.Core
{
    public class Fill : Command
    {
        public Fill(Canvas grid) : base(grid, "_Fill") { }
        public override IExecutable CreateOperation() => new Operation(Grid);

        private class Operation : Paint
        {
            public Operation(Canvas grid) : base(grid) { }

            protected override void Apply()
            {
                Canvas.Paint(GetFilledCells());
            }

            protected override Cell[] GetShadow() => GetArea().Select(c => c.Clone()).ToArray();

            private IEnumerable<Cell> GetFilledCells() => GetArea().Select(c => c.Clone(bg: Canvas.SelectedColor));

            private IEnumerable<Cell> GetArea()
            {
                bool[,] searched = new bool[Canvas.Size.X, Canvas.Size.Y];
                ConsoleColor[,] colors = new ConsoleColor[Canvas.Size.X, Canvas.Size.Y];
                for (int x = 0; x < Canvas.Size.X; x++)
                    for (int y = 0; y < Canvas.Size.Y; y++)
                        colors[x, y] = Canvas[(x, y)].Brush.Background;
                var seed = Canvas.CurrentPos;
                var targetColor = colors[seed.X, seed.Y];
                var area = new HashSet<Point>();
                var neighbours = new Stack<Point>(new[] { seed });
                while (neighbours.Any())
                {
                    var next = neighbours.Pop();
                    ScanLeft(next);
                    if (next.X > 0)
                        ScanRight(next);
                }
                return area.Select(p => Canvas[p]);

                void ScanLeft(Point p)
                {
                    Scan(GetPointsToFill(p.To(End(p))).ToArray());
                }

                void ScanRight(Point p)
                {
                    Scan(GetPointsToFill(p.Left.To(Start(p))).Reverse().ToArray());
                }

                IEnumerable<Point> GetPointsToFill(IEnumerable<Point> line)
                    => line.TakeWhile(p => colors[p.X, p.Y] == targetColor);

                void Scan(Point[] pointsToFill)
                {
                    if (!pointsToFill.Any())
                        return;
                    pointsToFill.ForEach(p =>
                    {
                        area.Add(p);
                        searched[p.X, p.Y] = true;
                    });
                    PushUpper(pointsToFill);
                    PushLower(pointsToFill);
                }

                void PushUpper(Point[] line)
                {
                    var y = line[0].Y - 1;
                    if (y < 0)
                        return;
                    PushNeighbours(line.Select(p => p.Up));
                }

                void PushLower(Point[] line)
                {
                    var y = line[0].Y + 1;
                    if (y == Canvas.Size.Y)
                        return;
                    PushNeighbours(line.Select(p => p.Down));
                }

                void PushNeighbours(IEnumerable<Point> line)
                {
                    var fillPrev = false;
                    foreach (var p in line)
                    {
                        if (searched[p.X, p.Y])
                            fillPrev = false;
                        else if (Canvas[p].Brush.Background != targetColor)
                        {
                            fillPrev = false;
                            searched[p.X, p.Y] = true;
                        }
                        else if (!fillPrev)
                        {
                            neighbours.Push(p);
                            fillPrev = true;
                        }
                    }
                }

                Point Start(Point p) => (0, p.Y);

                Point End(Point p) => (Canvas.Size.X - 1, p.Y);
            }
        }
    }
}