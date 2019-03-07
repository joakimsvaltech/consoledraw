using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.Core
{
    public class Rectangle
    {
        private Point end;

        public Rectangle(Point start, Point? end = null) => (Start, End) = (start, end ?? start);

        public Point Start { get; }
        public Point End
        {
            get => end; set
            {
                end = value;
                Size = End - Start;
            }
        }
        public Point Size { get; private set; }

        public Point[] Points
            => Start.To(Start * Size.Y)
            .SelectMany(left => left.To(left + Size.X))
            .ToArray();

        public Point[] Outline
                => Start.To(Start + Size.X)
                    .Concat(Start.To(Start * Size.Y))
                    .Concat((Start + Size.X).To((Start + Size.X) * Size.Y))
                    .Concat((Start * Size.Y).To(Start * Size.Y + Size.X))
            .Distinct()
            .ToArray();
    }
}