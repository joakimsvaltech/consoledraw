using System.Linq;

namespace ConsoleDraw.Core.Shapes
{
    public class Line : IShape
    {
        private Point _end;
        private readonly Point _start;

        public Line(Point start, Point? end = null) => (_start, _end) = (start, end ?? start);

        public void Update(Point point)
        {
            _end = point;
        }

        public Point[] Area => _start.To(_end).ToArray();

        public Point[] Outline => Area;
    }
}