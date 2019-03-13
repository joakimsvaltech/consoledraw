using ConsoleDraw.Core;
using ConsoleDraw.Core.Storage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Rectangle = ConsoleDraw.Geometry.Rectangle;
using Point = ConsoleDraw.Core.Geometry.Point;

namespace Storage
{
    public class ConsoleImage : IImage
    {
        public const byte WidthZoomFactor = 2;
        public const byte HeightZoomFactor = 4;

        public ConsoleImage(IImage image)
        {
            Cells = image.Cells;
            Size = image.Size;
        }

        public ConsoleImage(Image img)
        {
            var bmp = new Bitmap(img);
            Size = new Point(img.Width / WidthZoomFactor, img.Height / HeightZoomFactor);
            Cells = new Rectangle(0, 0, Size.X, Size.Y).Area
                .Select(p => new Cell {
                    Pos = p,
                    Color = Average(GetPixels(bmp, p)).ToConsoleColor()
                });
        }

        public Point Size { get; }

        public Bitmap ToBitmap()
        {
            var bmp = new Bitmap(Size.X * WidthZoomFactor, Size.Y * HeightZoomFactor);
            Cells.ForEach(cell => SetPixels(bmp, cell));
            return bmp;
        }

        private void SetPixels(Bitmap bmp, Cell cell)
        {
            var color = cell.Color.ToColor();
            GetSubpixelPoints(cell.Pos).ForEach(p => bmp.SetPixel(p.X, p.Y, color));
        }

        private Color Average(IEnumerable<Color> colors)
        {
            var red = colors.Average(c => c.R);
            var green = colors.Average(c => c.G);
            var blue = colors.Average(c => c.B);
            return Color.FromArgb((byte)Math.Round(red), (byte)Math.Round(green), (byte)Math.Round(blue));
        }

        private IEnumerable<Color> GetPixels(Bitmap bmp, Point p)
            => GetSubpixelPoints(p).Select(p => bmp.GetPixel(p.X, p.Y));

        private IEnumerable<Point> GetSubpixelPoints(Point p)
            => new Rectangle(p.X * WidthZoomFactor, p.Y * HeightZoomFactor, WidthZoomFactor, HeightZoomFactor).Area;

        public IEnumerable<Cell> Cells { get; }
    }
}