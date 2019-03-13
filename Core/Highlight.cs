using ConsoleDraw.Core.Geometry;
using System;
using System.Linq;

namespace ConsoleDraw.Core
{
    public class Highlight
    {
        public event EventHandler<EventArgs<Point[]>> AreaChanged;

        private Point[] _area = new Point[0];

        public Point[] Area
        {
            get => _area;
            set
            {
                if (value.SequenceEqual(_area)) return;
                _area = value;
                AreaChanged?.Invoke(this, Area);
            }
        }
    }
}