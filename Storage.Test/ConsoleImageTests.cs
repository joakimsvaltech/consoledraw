using ConsoleDraw.Core;
using ConsoleDraw.Geometry;
using Storage;
using System;
using System.Drawing;
using System.Linq;
using Xunit;
using static Xunit.Assert;

namespace ConsoleDraw.Storage.Test
{
    public class ConsoleImageTests
    {
        [Theory]
        [InlineData(1, 1)]
        [InlineData(3, 3)]
        public void GenerateBitmapFromCanvas(int width, int height)
        {
            var canvas = new Canvas((width, height), 16);
            var diagonal = canvas.Size.Up.Left.To((0, 0)).ToArray();
            diagonal.ForEach(p => canvas[p].Brush = ConsoleColor.Red);
            var bmp = new ConsoleImage(canvas).ToBitmap();
            foreach (var p in diagonal)
            {
                Equal(Color.FromArgb(Color.Red.ToArgb()), bmp.GetPixel(p.X * ConsoleImage.WidthZoomFactor, p.Y * ConsoleImage.HeightZoomFactor));
                Equal(Color.FromArgb(Color.Red.ToArgb()), bmp.GetPixel(p.X * ConsoleImage.WidthZoomFactor + 1, p.Y * ConsoleImage.HeightZoomFactor + 1));
            }
        }
    }
}