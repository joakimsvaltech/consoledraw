using ConsoleDraw.Core.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.Geometry
{
    public static class PointExtensions
    {
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
    }
}