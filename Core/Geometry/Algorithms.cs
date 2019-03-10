using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.Core.Geometry
{
    public static class Algorithms
    {
        public static IEnumerable<Cell> FindLargestConnectedArea(this Canvas grid)
            => FindConnectedAreas(grid)
            .OrderByDescending(area => area.Length)
            .FirstOrDefault();

        public static IEnumerable<Cell> GetArea(this Canvas grid, Point center)
        {
            var next = grid[center];
            var color = next.Color;
            var area = new HashSet<Cell> { next };
            var neighbours = new Stack<Cell>(next.Neighbours(grid).Where(n => n.Color == color));
            while (neighbours.Any())
            {
                next = neighbours.Pop();
                area.Add(next);
                next.Neighbours(grid).Where(n => n.Color == color).Except(area).ForEach(n => neighbours.Push(n));
            }
            return area;
        }

        public static IEnumerable<Point> To(this Point from, Point to)
            => from == to ? new[] { from } : IsSteep(from, to) ? SteepTo(from, to) : FlatTo(from, to);

        private static bool IsSteep(Point from, Point to)
            => Math.Abs(from.X - to.X) < Math.Abs(from.Y - to.Y);

        private static IEnumerable<Point> SteepTo(Point from, Point to)
            => FlatTo(from.Invert(), to.Invert()).Select(p => p.Invert());

        private static IEnumerable<Point> FlatTo(Point from, Point to)
        {
            var sign = from.X > to.X ? -1 : 1;
            return Offsets(from, to).Select((offset, i) => new Point(from.X + sign * i, from.Y + offset));
        }

        private static IEnumerable<int> Offsets(Point from, Point to)
        {
            var sign = from.Y > to.Y ? -1 : 1;
            var count = Math.Abs(to.X - from.X) + 1;
            var k = sign * (Math.Abs(to.Y - from.Y) + 1) / (double)count;
            return Enumerable.Range(0, count).Select(i => (int)((i + 0.5) * k));
        }

        private static Cell[][] FindConnectedAreas(Canvas grid)
        {
            var indices = new int[grid.Size.X + 1, grid.Size.Y + 1];
            var nextIndex = 1;
            foreach (var cell in grid.Cells)
            {
                var sameColoredNeighbours = grid.FindSameColoredNeighbours(cell);
                if (sameColoredNeighbours.Any())
                {
                    var connectedAreaIndices = sameColoredNeighbours.Select(n => indices[n.Pos.X, n.Pos.Y]).ToArray();
                    var minConnectedAreaIndex = connectedAreaIndices.Min();
                    var maxConnectedAreaIndex = connectedAreaIndices.Max();
                    if (maxConnectedAreaIndex > minConnectedAreaIndex)
                        indices.Replace(maxConnectedAreaIndex, minConnectedAreaIndex);
                    indices[cell.Pos.X, cell.Pos.Y] = minConnectedAreaIndex;
                }
                else
                {
                    indices[cell.Pos.X, cell.Pos.Y] = nextIndex++;
                }
            }
            var areas = new Dictionary<int, List<Cell>>();
            foreach (Point pos in grid.Positions)
            {
                var areaIndex = indices[pos.X, pos.Y];
                if (!areas.TryGetValue(areaIndex, out var area))
                    areas[areaIndex] = area = new List<Cell>();
                area.Add(grid[pos]);
            }
            return areas
                .OrderByDescending(p => p.Value.Count)
                .Select((p, i) => p.Value.Select(c => c.Clone((char)(65 + i))).ToArray())
                .ToArray();
        }

        private static Cell[] FindSameColoredNeighbours(this Canvas grid, Cell cell)
            => cell.NorthWestNeighbours(grid)
            .Where(n => n.Color == cell.Color).ToArray();

        private static void Replace(this int[,] grid, int replace, int with)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
                for (int y = 0; y < grid.GetLength(1); y++)
                    if (grid[x, y] == replace)
                        grid[x, y] = with;
        }
    }
}