using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.Core
{
    public class Cell
    {
        public Point Pos { get; set; }
        public ConsoleColor Color { get; set; }
        public char Tag { get; set; } = ' ';

        public IEnumerable<Cell> Neighbours(Grid grid)
            => NorthWestNeighbours(grid).Concat(SouthEastNeighbours(grid));

        public IEnumerable<Cell> NorthWestNeighbours(Grid grid)
        {
            if (Pos.X > 0) yield return grid[Pos.Left];
            if (Pos.Y > 0) yield return grid[Pos.Up];
        }

        public IEnumerable<Cell> SouthEastNeighbours(Grid grid)
        {
            if (Pos.X < grid.Size.X - 1) yield return grid[Pos.Right];
            if (Pos.Y < grid.Size.Y - 1) yield return grid[Pos.Down];
        }

        public Cell Clone(char tag)
            => new Cell
            {
                Color = Color,
                Tag = tag,
                Pos = Pos
            };

        public override string ToString() => $"{Pos}:{Tag}{(int)Color}";
    }
}