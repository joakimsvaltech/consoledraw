using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.Core
{
    public static class Algorithms
    {
        public static IEnumerable<Cell> FindLargestConnectedArea(this Grid grid)
            => FindConnectedAreas(grid)
            .OrderByDescending(area => area.Length)
            .FirstOrDefault();

        public static IEnumerable<Cell> GetArea(this Grid grid, Point center)
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
            => from == to 
            ? new [] { from}
            : IsSteep(from, to)
            ? SteepTo(from, to, from.X > to.X ? -1 : 1)
            : FlatTo(from, to, from.Y > to.Y ? -1 : 1);

        private static bool IsSteep(Point from, Point to)
            => Math.Abs(from.X - to.X) < Math.Abs(from.Y - to.Y);

        private static IEnumerable<Point> FlatTo(Point from, Point to, int sign)
        {
            var k = (to.Y - from.Y) / (double)Math.Abs(to.X - from.X);
            return Enumerable.Range(0, Math.Abs(to.X - from.X) + 1)
                       .Select(i => new Point(from.X + sign * i, (int)(from.Y + i * k)));
        }

        private static IEnumerable<Point> SteepTo(Point from, Point to, int sign)
        {
            var k = (to.X - from.X) / (double)Math.Abs(to.Y - from.Y);
            return Enumerable.Range(0, Math.Abs(to.Y - from.Y) + 1)
            .Select(i => new Point((int)(from.X + i * k), from.Y + sign * i));
        }

        private static Cell[][] FindConnectedAreas(Grid grid)
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

        private static Cell[] FindSameColoredNeighbours(this Grid grid, Cell cell)
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