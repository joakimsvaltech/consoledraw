using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.Core.Shapes
{
    public class Doodle : IShape
    {
        private readonly List<Point> _points;
        public Doodle(params Point[] points) => _points = points.ToList();
        public Point[] Area => _points.Distinct().ToArray();
        public Point[] Outline => Area;
        public void Update(Point point) => _points.Add(point);
    }
}