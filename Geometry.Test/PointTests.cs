using ConsoleDraw.Core.Geometry;
using System.Linq;
using Xunit;
using static Xunit.Assert;

namespace ConsoleDraw.Geometry.Test
{
    public class PointTests
    {
        [Theory]
        [InlineData(0, 0, 0, 0, 0)]
        [InlineData(1, 1, 1, 2, 1)]
        [InlineData(1, 2, 3, 4, 2)]
        public void When_Plus_OffsetX(int x, int y, int offset, int expectedX, int expectedY)
        {
            Point point = (x, y);
            var result = point + offset;
            Equal(result, (expectedX, expectedY));
        }

        [Theory]
        [InlineData(0, 0, 0, 0, 0)]
        [InlineData(1, 1, 1, 1, 2)]
        [InlineData(1, 2, 3, 1, 5)]
        public void When_Multiply_OffsetY(int x, int y, int offset, int expectedX, int expectedY)
        {
            Point point = (x, y);
            var result = point * offset;
            Equal(result, (expectedX, expectedY));
        }

        [Theory]
        [InlineData(0, 0, 0, 0, 0, 0)]
        [InlineData(1, 1, 1, 1, 2, 2)]
        [InlineData(1, 2, 3, 4, 4, 6)]
        public void When_Add_Offset_X_And_Y(int x, int y, int offsetX, int offsetY, int expectedX, int expectedY)
        {
            Point point = (x, y);
            var result = point + (offsetX, offsetY);
            Equal(result, (expectedX, expectedY));
        }

        [Theory]
        [InlineData(0, 0, 1, 1, 0, 0)]
        [InlineData(1, 1, 2, 2, 1, 1)]
        [InlineData(6, 5, 4, 3, 2, 2)]
        public void When_Modulo_Then_Modulo_X_And_Y(int x, int y, int modX, int modY, int expectedX, int expectedY)
        {
            Point point = (x, y);
            var result = point % (modX, modY);
            Equal(result, (expectedX, expectedY));
        }

        [Theory]
        [InlineData(0, 0, 0, 0,
            0, 0)]
        [InlineData(0, 0, 1, 0,
            0, 0, 1, 0)]
        [InlineData(0, 0, 2, 0,
            0, 0, 1, 0, 2, 0)]
        [InlineData(0, 0, 0, 1,
            0, 0, 0, 1)]
        [InlineData(2, 3, 2, 4,
            2, 3, 2, 4)]
        [InlineData(2, 3, 0, 3,
            2, 3, 1, 3, 0, 3)]
        [InlineData(3, 3, 2, 2,
            3, 3, 2, 2)]
        [InlineData(3, 3, 4, 2,
            3, 3, 4, 2)]
        [InlineData(3, 3, 2, 4,
            3, 3, 2, 4)]
        [InlineData(3, 3, 4, 4,
            3, 3, 4, 4)]
        [InlineData(3, 3, 6, 5,
            3, 3, 4, 4, 5, 4, 6, 5)]
        [InlineData(3, 3, 6, 1,
            3, 3, 4, 2, 5, 2, 6, 1)]
        [InlineData(3, 3, 6, 4,
            3, 3, 4, 3, 5, 4, 6, 4)]
        public void To_return_inclusive_range_between_points(int x, int y, int endX, int endY,
            params int[] expectedCoords)
        {
            Point start = (x, y);
            var result = start.To((endX, endY)).ToArray();
            var expected = TestUtils.GetPoints(expectedCoords);
            Equal(expected, result);
        }
    }
}