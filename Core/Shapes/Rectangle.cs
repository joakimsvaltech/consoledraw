using System;
using System.Linq;

namespace ConsoleDraw.Core.Shapes
{
    public class Rectangle : IShape
    {
        private Point _end;
        private readonly Point _start;
        private Point _size;

        public Rectangle(Point start, Point? end = null) => (_start, End) = (start, end ?? start);

        public void Update(Point point)
        {
            End = point;
        }

        private Point End
        {
            set
            {
                _end = value;
                _size = new Point(Math.Abs(_end.X - _start.X), Math.Abs(_end.Y - _start.Y));
            }
        }

        public Point[] Area
            => _start.To(_start * _size.Y)
            .SelectMany(left => left.To(left + _size.X))
            .ToArray();

        public Point[] Outline
                => _start.To(_start + _size.X)
                    .Concat(_start.To(_start * _size.Y))
                    .Concat((_start + _size.X).To((_start + _size.X) * _size.Y))
                    .Concat((_start * _size.Y).To(_start * _size.Y + _size.X))
            .Distinct()
            .ToArray();
    }
}