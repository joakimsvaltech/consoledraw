using System;
using System.Linq;

namespace ConsoleDraw.Core.Geometry
{
    public class Rectangle : IShape
    {
        private Point _end;
        private readonly Point _start;

        private int Top => Math.Min(_start.Y, _end.Y);
        private int Bottom => Math.Max(_start.Y, _end.Y);
        private int Left => Math.Min(_start.X, _end.X);
        private int Right => Math.Max(_start.X, _end.X);
        private int Width => Right - Left;
        private Point UpperLeft => new Point(Left, Top);
        private Point LowerLeft => new Point(Left, Bottom);
        private Point UpperRight => new Point(Right, Top);
        private Point LowerRight => new Point(Right, Bottom);

        public Rectangle(Point start, Point? end = null) => (_start, _end) = (start, end ?? start);

        public void Update(Point point)
        {
            _end = point;
        }

        public Point[] Area
            => UpperLeft.To(LowerLeft)
            .SelectMany(left => left.To(left + Width))
            .ToArray();

        public Point[] Outline
                => UpperLeft.To(UpperRight)
                    .Concat(UpperLeft.To(LowerLeft))
                    .Concat(UpperRight.To(LowerRight))
                    .Concat(LowerLeft.To(LowerRight))
            .Distinct()
            .ToArray();
    }
}