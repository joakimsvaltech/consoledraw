using ConsoleDraw.Core;
using ConsoleDraw.Core.Geometry;
using System.Linq;

namespace Core.Test
{
    public static class TestUtils
    {
        public static Point[] GetPoints(params int[] expectedCoords)
            => expectedCoords.Even().Combine(expectedCoords.Odd(), (x, y) => new Point(x, y)).ToArray();
    }
}
