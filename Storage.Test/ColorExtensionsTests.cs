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
        [InlineData(ConsoleColor.Black, 0x000000)]
        [InlineData(ConsoleColor.White, 0xFFFFFF)]
        [InlineData(ConsoleColor.Red, 0xFF0000)]
        [InlineData(ConsoleColor.DarkGreen, 0x008000)]
        [InlineData(ConsoleColor.DarkCyan, 0x008080)]
        [InlineData(ConsoleColor.Gray, 0xC0C0C0)]
        [InlineData(ConsoleColor.DarkGray, 0x808080)]
        public void ConsoleColorToColor(ConsoleColor target, int expected)
            => Equal(Color.FromArgb(expected), target.ToColor());

        [Theory]
        [InlineData(ConsoleColor.Black, 0x000000)]
        [InlineData(ConsoleColor.White, 0xFFFFFF)]
        [InlineData(ConsoleColor.Red, 0xFF0000)]
        [InlineData(ConsoleColor.DarkGreen, 0x008000)]
        [InlineData(ConsoleColor.DarkCyan, 0x008080)]
        [InlineData(ConsoleColor.Gray, 0xC0C0C0)]
        [InlineData(ConsoleColor.DarkGray, 0x808080)]
        [InlineData(ConsoleColor.Yellow, 0xC0C030)]
        [InlineData(ConsoleColor.Blue, 0x949EB0)]
        public void ColorToConsoleColor(ConsoleColor expected, int target)
            => Equal(expected, Color.FromArgb(target).ToConsoleColor());
    }
}