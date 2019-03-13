using ConsoleDraw.Core.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.Core
{
    public class Cell : IEquatable<Cell?>
    {
        public Point Pos { get; set; }
        public Brush Brush { get; set; }

        public IEnumerable<Cell> Neighbours(Canvas grid)
            => NorthWestNeighbours(grid).Concat(SouthEastNeighbours(grid));

        public IEnumerable<Cell> NorthWestNeighbours(Canvas grid)
        {
            if (Pos.X > 0) yield return grid[Pos.Left];
            if (Pos.Y > 0) yield return grid[Pos.Up];
        }

        public IEnumerable<Cell> SouthEastNeighbours(Canvas grid)
        {
            if (Pos.X < grid.Size.X - 1) yield return grid[Pos.Right];
            if (Pos.Y < grid.Size.Y - 1) yield return grid[Pos.Down];
        }

        public Cell Clone(char? tag = null, ConsoleColor? bg = null, ConsoleColor? fg = null)
            => new Cell
            {
                Brush =  (bg ?? Brush.Background, fg ?? Brush.Foreground, tag ?? Brush.Shape),
                Pos = Pos
            };

        public static bool operator ==(Cell left, Cell right)
            => left.Equals(right);

        public static bool operator !=(Cell left, Cell right)
            => !left.Equals(right);

        public override string ToString() => $"{Pos}:{Brush}";

        public bool Equals(Cell? other)
            => !(other is null) && other.Pos == Pos && other.Brush == Brush;

        public override bool Equals(object obj)
            => Equals(obj as Cell);

        public override int GetHashCode()
            => Pos.GetHashCode() + Brush.GetHashCode();
    }
}