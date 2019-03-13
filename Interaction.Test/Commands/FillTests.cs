using ConsoleDraw.Core;
using ConsoleDraw.Geometry.Test;
using System;
using System.Linq;
using Xunit;
using static Xunit.Assert;

namespace ConsoleDraw.Interaction.Test.Commands
{
    public class FillTests
    {
        [Theory]
        [InlineData(1, 10, 10, 5, 5)]
        [InlineData(10, 10, 10, 5, 5
            , 2, 2, 3, 2, 4, 2, 2, 6, 3, 7, 4, 8, 5, 9)]
        [InlineData(200, 100, 100, 50, 50)]
        public void FillEfficiently(int expectedMs, int width, int height, int x, int y
            , params int[] obstacleCoords)
        {
            var canvas = new Canvas((width, height), 16);
            var cmd = new Fill(canvas);
            var op = cmd.CreateOperation();
            var obstacles = TestUtils.GetPoints(obstacleCoords);
            obstacles.ForEach(o => canvas.Plot(new Cell
            {
                Pos = o,
                Brush = ConsoleColor.Green
            }));
            canvas.SelectedColor = ConsoleColor.Red;
            canvas.CurrentPos = (x, y);
            var before = DateTime.Now;
            op.Execute();
            var after = DateTime.Now;
            True(canvas.Cells.Where(c => !obstacles.Contains(c.Pos)).All(c => c.Brush.Background == ConsoleColor.Red));
            True(canvas.Cells.Where(c => obstacles.Contains(c.Pos)).All(c => c.Brush.Background == ConsoleColor.Green));
            var durationMs = (after - before).TotalMilliseconds;
            True(durationMs < expectedMs, $"Took milliseconds: {durationMs}");
        }
    }
}