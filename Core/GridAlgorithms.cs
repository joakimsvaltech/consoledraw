using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.Core
{
    public static class GridAlgorithms
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

        public static IEnumerable<Point> HorisontalTo(this Point start, Point end, int sign)
            => Enumerable.Range(0, Math.Abs(end.X - start.X) + 1)
            .Select(i => new Point(start.X + sign * i, start.Y));

        public static IEnumerable<Point> VerticalTo(this Point start, Point end, int sign)
            => Enumerable.Range(0, Math.Abs(end.Y - start.Y) + 1)
            .Select(i => new Point(start.X, start.Y + sign * i));

        private static Cell[][] FindConnectedAreas(Grid grid)
        {
            var indices = new int[grid.Size.X, grid.Size.Y];
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