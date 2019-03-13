using ConsoleDraw.Core;
using ConsoleDraw.Core.Geometry;
using ConsoleDraw.Geometry;
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
        [InlineData(5, 10, 10)]
        [InlineData(10, 5, 5)]
        [InlineData(100, 100, 100)]
        [InlineData(400, 200, 200)]
        [InlineData(800, 300, 300)]
        public void FillEfficiently(int expectedMs, int w, int h)
        {
            var x = 1;
            var y = h / 2;
            var canvas = new Canvas((w, h), 16);
            var cmd = new Fill(canvas);
            var op = cmd.CreateOperation();
            var obstacles = new Point(1, 1).To((w - 2, h - 2))
                .Concat(new Point(w - 2, 1).To((1, h - 2)))
                .ToArray();
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