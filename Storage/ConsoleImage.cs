using ConsoleDraw.Core;
using ConsoleDraw.Core.Storage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Rectangle = ConsoleDraw.Geometry.Rectangle;

namespace Storage
{
    internal class ConsoleImage : IImage
    {
        private const byte WidthZoomFactor = 2;
        private const byte HeightZoomFactor = 4;

        public ConsoleImage(Image img)
        {
            var bmp = new Bitmap(img);
            Cells = new Rectangle(0, 0, 
                (img.Width - 1) / WidthZoomFactor, 
                (img.Height - 1) / HeightZoomFactor)
                .Area
                .Select(p => new Cell {
                    Pos = p,
                    Color = Average(GetPixels(bmp, p.X, p.Y)).ToConsoleColor()
                }).ToArray();
        }

        private Color Average(IEnumerable<Color> colors)
        {
            var red = colors.Average(c => c.R);
            var green = colors.Average(c => c.G);
            var blue = colors.Average(c => c.B);
            return Color.FromArgb((byte)Math.Round(red), (byte)Math.Round(green), (byte)Math.Round(blue));
        }

        private IEnumerable<Color> GetPixels(Bitmap bmp, int x, int y) 
            => new Rectangle(
                x * WidthZoomFactor, 
                y * HeightZoomFactor, 
                WidthZoomFactor, 
                HeightZoomFactor)
            .Area
            .Where(p => p.X < bmp.Width && p.Y < bmp.Height)
            .Select(p => bmp.GetPixel(p.X, p.Y));

        public Cell[] Cells { get; set; }
    }
}