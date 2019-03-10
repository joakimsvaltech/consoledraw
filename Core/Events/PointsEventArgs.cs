using ConsoleDraw.Core.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.Core.Events
{
    public class PointsEventArgs : EventArgs
    {
        public PointsEventArgs(IEnumerable<Point> points) => Points = points.ToArray();
        public Point[] Points { get; }
    }
}