using Storage;
using System;
using System.Drawing;
using Xunit;
using static Xunit.Assert;

namespace ConsoleDraw.Storage.Test
{
    public class ColorExtensionsTests
    {
        [Theory]
        [InlineData(ConsoleColor.Black, 0xFF000000)]
        [InlineData(ConsoleColor.White, 0xFFFFFFFF)]
        [InlineData(ConsoleColor.Red, 0xFFFF0000)]
        [InlineData(ConsoleColor.DarkGreen, 0xFF008000)]
        [InlineData(ConsoleColor.DarkCyan, 0xFF008080)]
        [InlineData(ConsoleColor.Gray, 0xFFC0C0C0)]
        [InlineData(ConsoleColor.DarkGray, 0xFF808080)]
        public void ConsoleColorToColor(ConsoleColor target, uint expected)
            => Equal(Color.FromArgb((int)expected), target.ToColor());

        [Theory]
        [InlineData(ConsoleColor.Black, 0xFF000000)]
        [InlineData(ConsoleColor.White, 0xFFFFFFFF)]
        [InlineData(ConsoleColor.Red, 0xFFFF0000)]
        [InlineData(ConsoleColor.DarkGreen, 0xFF008000)]
        [InlineData(ConsoleColor.DarkCyan, 0xFF008080)]
        [InlineData(ConsoleColor.Gray, 0xFFC0C0C0)]
        [InlineData(ConsoleColor.DarkGray, 0xFF808080)]
        [InlineData(ConsoleColor.Yellow, 0xFFC0C030)]
        [InlineData(ConsoleColor.Blue, 0xFF949EB0)]
        public void ColorToConsoleColor(ConsoleColor expected, uint target)
            => Equal(expected, Color.FromArgb((int)target).ToConsoleColor());
    }
}