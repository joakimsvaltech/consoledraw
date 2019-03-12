using System;
using System.Drawing;

namespace Storage
{
    internal class ColorMappingFailed : Exception
    {
        public Color Color { get; }
        public ColorMappingFailed(Color c) => Color = c;
    }
}