using System;
using System.Drawing;
using System.Linq;

namespace Storage
{
    public static class ColorExtensions
    {
        private static readonly int[] CColors = {
            0x000000, //Black = 0
            0x000080, //DarkBlue = 1
            0x008000, //DarkGreen = 2
            0x008080, //DarkCyan = 3
            0x800000, //DarkRed = 4
            0x800080, //DarkMagenta = 5
            0x808000, //DarkYellow = 6
            0xC0C0C0, //Gray = 7
            0x808080, //DarkGray = 8
            0x0000FF, //Blue = 9
            0x00FF00, //Green = 10
            0x00FFFF, //Cyan = 11
            0xFF0000, //Red = 12
            0xFF00FF, //Magenta = 13
            0xFFFF00, //Yellow = 14
            0xFFFFFF  //White = 15
        };

        public static Color ToColor(this ConsoleColor c) => Color.FromArgb(CColors[(int)c]);

        public static ConsoleColor ToConsoleColor(this Color c)
        {
            if (IsGray(c.R) && IsGray(c.G) && IsGray(c.B))
                return ConsoleColor.Gray;
            var red = GetClosest(c.R);
            var green = GetClosest(c.G);
            var blue = GetClosest(c.B);
            if (new[] { red, green, blue }.Max() == Max)
            {
                MaximizeContrast(ref red);
                MaximizeContrast(ref green);
                MaximizeContrast(ref blue);
            }
            var argb = (red << 16) + (green << 8) + blue;
            var colorIndex = Array.IndexOf(CColors, argb);
            return colorIndex >= 0 ? (ConsoleColor)colorIndex : throw new ColorMappingFailed(c);
        }

        private static void MaximizeContrast(ref byte c) => c = c == Max ? Max : Min;

        private static bool IsGray(byte c) => c >= LowerGreyBound && c < UpperGreyBound;

        private static byte GetClosest(byte c) => c < MinBound ? Min : c < LowBound ? Low : Max;

        private const byte Min = 0;
        private const byte MinBound = 0x50;
        private const byte Low = 0x80;
        private const byte LowerGreyBound = 0xA0;
        private const byte LowBound = 0xB0;
        private const byte UpperGreyBound = 0xE0;
        private const byte Max = 0xFF;
    }
}