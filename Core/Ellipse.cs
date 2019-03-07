using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.Core
{
    public class Ellipse : IShape
    {
        private Point end;

        public Ellipse(Point start, Point? end = null) => (Start, End) = (start, end ?? start);

        public Point Start { get; }
        public Point End
        {
            get => end; set
            {
                end = value;
                Size = new Point(Math.Abs(End.X - Start.X), Math.Abs(End.Y - Start.Y));
            }
        }
        public Point Size { get; private set; }

        public Point[] Points => ComputePoints().Distinct().ToArray();

        public Point[] Outline => ComputeOutline().Distinct().ToArray();

        private IEnumerable<Point> ComputePoints()
        {
            var height = Size.Y + 1;
            var width = Size.X + 1;
            var startX = Math.Min(Start.X, End.X);
            var startY = Math.Min(Start.Y, End.Y);
            var endX = Math.Max(Start.X, End.X);
            var endY = Math.Max(Start.Y, End.Y);
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
                for (var x = offset; x < width / 2; x++) {
                    yield return new Point(startX + x, startY + y);
                    yield return new Point(endX - x, startY + y);
                    yield return new Point(startX + x, endY - y);
                    yield return new Point(endX - x, endY - y);
                }
            }
        }

        private IEnumerable<Point> ComputeOutline()
        {
            var height = Size.Y + 1;
            var width = Size.X + 1;
            var startX = Math.Min(Start.X, End.X);
            var startY = Math.Min(Start.Y, End.Y);
            var endX = Math.Max(Start.X, End.X);
            var endY = Math.Max(Start.Y, End.Y);
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