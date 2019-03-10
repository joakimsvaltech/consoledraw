using ConsoleDraw.Core.Geometry;
using System;

namespace ConsoleDraw.Core.Events
{
    public class PointEventArgs : EventArgs
    {
        public PointEventArgs(Point point) => Point = point;
        public Point Point { get; }
    }
}