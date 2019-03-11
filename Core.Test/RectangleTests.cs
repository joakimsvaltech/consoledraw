using ConsoleDraw.Core.Geometry;
using ConsoleDraw.Geometry;
using Xunit;

namespace ConsoleDraw.Geometry.Test
{
    public class RectangleTests
    {
        [Theory]
        [InlineData(0, 0, 1, 1,
            0, 0,
            1, 0,
            0, 1,
            1, 1)]
        [InlineData(1, 1, 2, 3,
            1, 1,
            2, 1,
            1, 2,
            2, 2,
            1, 3,
            2, 3)]
        [InlineData(4, 4, 2, 3,
            4, 4,
            3, 4,
            2, 4,
            4, 3,
            3, 3,
            2, 3)]
        public void Points_return_all_inner_points_including_edge(int startX, int startY, int endX, int endY,
            params int[] expectedCoords)
        {
            var start = new Point(startX, startY);
            var end = new Point(endX, endY);
            var rect = new Rectangle(start, end);
            var actual = rect.Area;
            var expected = TestUtils.GetPoints(expectedCoords);
            Assert.Equal(actual, expected);
        }

        [Theory]
        [InlineData(0, 0, 1, 1,
            0, 0,
            1, 0,
            0, 1,
            1, 1)]
        [InlineData(1, 1, 2, 3,
            1, 1,
            2, 1,
            1, 2,
            1, 3,
            2, 2,
            2, 3)]
        [InlineData(4, 4, 2, 2,
            4, 4,
            3, 4,
            2, 4,
            4, 3,
            4, 2,
            2, 3,
            2, 2,
            3, 2)]
        public void Outline_return_all_points_on_the_edge(int startX, int startY, int endX, int endY,
            params int[] expectedCoords)
        {
            var start = new Point(startX, startY);
            var end = new Point(endX, endY);
            var rect = new Rectangle(start, end);
            var actual = rect.Outline;
            var expected = TestUtils.GetPoints(expectedCoords);
            Assert.Equal(actual, expected);
        }
    }
}