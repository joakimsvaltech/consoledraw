using ConsoleDraw.Core.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.Geometry
{
    public class Ellipse : IShape
    {
        private Point _end;
        private readonly Point _start;
        private Point _size;

        public Ellipse(Point start, Point? end = null) => (_start, End) = (start, end ?? start);

        public Point End
        {
            set
            {
                _end = value;
                _size = new Point(Math.Abs(_end.X - _start.X), Math.Abs(_end.Y - _start.Y));
            }
        }

        public void Update(Point point) => End = point;

        public Point[] Area => ComputePoints().Distinct().ToArray();

        public Point[] Outline => ComputeOutline().Distinct().ToArray();

        private IEnumerable<Point> ComputePoints()
        {
            var height = _size.Y + 1;
            var width = _size.X + 1;
            var startX = Math.Min(_start.X, _end.X);
            var startY = Math.Min(_start.Y, _end.Y);
            var endX = Math.Max(_start.X, _end.X);
            var endY = Math.Max(_start.Y, _end.Y);
            if (height % 2 == 1)
            {
                var midY = startY + height / 2;
                for (var x = startX; x <= endX; x++)
                    yield return new Point(x, midY);
            }
            if (width % 2 == 1)
            {
                var midX = startX + width / 2;
                for (var y = startY; y <= endY; y++)
                    yield return new Point(midX, y);
            }
            for (var y = 0; y < height / 2; y++)
            {
                var offset = ComputeOffset(y, width, height);
                for (var x = offset; x < width / 2; x++)
                {
                    yield return new Point(startX + x, startY + y);
                    yield return new Point(endX - x, startY + y);
                    yield return new Point(startX + x, endY - y);
                    yield return new Point(endX - x, endY - y);
                }
            }
        }

        private IEnumerable<Point> ComputeOutline()
        {
            var height = _size.Y + 1;
            var width = _size.X + 1;
            var startX = Math.Min(_start.X, _end.X);
            var startY = Math.Min(_start.Y, _end.Y);
            var endX = Math.Max(_start.X, _end.X);
            var endY = Math.Max(_start.Y, _end.Y);
            var prevOffset = width / 2;
            for (var y = 0; y <= height / 2; y++)
            {
                var offset = ComputeOffset(y, width, height);
                for (var x = offset; x <= prevOffset; x++)
                {
                    yield return new Point(startX + x, startY + y);
                    yield return new Point(endX - x, startY + y);
                    yield return new Point(startX + x, endY - y);
                    yield return new Point(endX - x, endY - y);
                }
                prevOffset = offset;
            }
        }

        private int ComputeOffset(int y, int width, int height)
        {
            if (height == 1) return 0;
            var normY = Math.Abs(2 * (0.5 - (y + 0.5) / height));
            var normX = Math.Sqrt(1 - normY * normY);
            var normOffset = 1 - normX;
            return (int)(normOffset * width / 2);
        }
    }
}