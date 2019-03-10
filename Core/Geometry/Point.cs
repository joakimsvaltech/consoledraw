using System;

namespace ConsoleDraw.Core.Geometry
{
    public readonly struct Point : IEquatable<Point>
    {
        public Point(int x, int y) => (X, Y) = (x, y);

        public static bool operator ==(Point left, Point right)
            => left.Equals(right);

        public static bool operator !=(Point left, Point right)
            => !left.Equals(right);

        public static Point operator +(Point left, Point right)
            => new Point(left.X + right.X, left.Y + right.Y);

        public static Point operator -(Point left, Point right)
            => new Point(left.X - right.X, left.Y - right.Y);

        public static Point operator +(Point left, int offsetX)
            => new Point(left.X + offsetX, left.Y);

        public static Point operator *(Point left, int offsetY)
            => new Point(left.X, left.Y + offsetY);

        public static Point operator %(Point left, Point right)
            => new Point(left.X % right.X, left.Y % right.Y);

        public int X { get; }
        public int Y { get; }

        public Point Up => new Point(X, Y - 1);

        public Point Down => new Point(X, Y + 1);

        public Point Left => new Point(X - 1, Y);

        public Point Right => new Point(X + 1, Y);

        public Point Invert() => new Point(Y, X);

        public override bool Equals(object obj)
            => obj is Point p && Equals(p);

        public bool Equals(Point other)
            => other.X == X && other.Y == Y;

        public override int GetHashCode()
            => X + Y;

        public override string ToString() => $"({X},{Y})";

        internal Point Neighbour(Direction direction) => direction switch
        {
            Direction.Up => Up,
            Direction.Down => Down,
            Direction.Left => Left,
            Direction.Right => Right,
            _ => throw new NotImplementedException()
        };

        public Point Diagonal(int offset) => new Point(X + offset, Y + offset);
    }
}