using ConsoleDraw.Core;
using System.Linq;
using Xunit;

namespace Core.Test
{
    public class PointTests
    {
        [Theory]
        [InlineData(0, 0, 0, 0, 0)]
        [InlineData(1, 1, 1, 2, 1)]
        [InlineData(1, 2, 3, 4, 2)]
        public void When_Plus_OffsetX(int x, int y, int offset, int expectedX, int expectedY)
        {
            var point = new Point(x, y);
            var result = point + offset;
            Assert.Equal(result, new Point(expectedX, expectedY));
        }

        [Theory]
        [InlineData(0, 0, 0, 0, 0)]
        [InlineData(1, 1, 1, 1, 2)]
        [InlineData(1, 2, 3, 1, 5)]
        public void When_Multiply_OffsetY(int x, int y, int offset, int expectedX, int expectedY)
        {
            var point = new Point(x, y);
            var result = point * offset;
            Assert.Equal(result, new Point(expectedX, expectedY));
        }

        [Theory]
        [InlineData(0, 0, 0, 0, 0, 0)]
        [InlineData(1, 1, 1, 1, 2, 2)]
        [InlineData(1, 2, 3, 4, 4, 6)]
        public void When_Add_Offset_X_And_Y(int x, int y, int offsetX, int offsetY, int expectedX, int expectedY)
        {
            var point = new Point(x, y);
            var offset = new Point(offsetX, offsetY);
            var result = point + offset;
            Assert.Equal(result, new Point(expectedX, expectedY));
        }

        [Theory]
        [InlineData(0, 0, 1, 1, 0, 0)]
        [InlineData(1, 1, 2, 2, 1, 1)]
        [InlineData(6, 5, 4, 3, 2, 2)]
        public void When_Modulo_Then_Modulo_X_And_Y(int x, int y, int modX, int modY, int expectedX, int expectedY)
        {
            var point = new Point(x, y);
            var offset = new Point(modX, modY);
            var result = point % offset;
            Assert.Equal(result, new Point(expectedX, expectedY));
        }

        [Theory]
        [InlineData(0, 0, 0, 0, 0, 0)]
        [InlineData(0, 0, 1, 0, 0, 0, 1, 0)]
        [InlineData(0, 0, 2, 0, 0, 0, 1, 0, 2, 0)]
        [InlineData(0, 0, 0, 1, 0, 0, 0, 1)]
        [InlineData(2, 3, 2, 4, 2, 3, 2, 4)]
        [InlineData(2, 3, 0, 3, 2, 3, 1, 3, 0, 3)]
        public void To_return_inclusive_range_between_points(int x, int y, int endX, int endY, params int[] expectedCoords)
        {
            var start = new Point(x, y);
            var end = new Point(endX, endY);
            var result = start.To(end);
            var expected = TestUtils.GetPoints(expectedCoords);
            Assert.Equal(result, expected);
        }
    }
}