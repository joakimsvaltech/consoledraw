using ConsoleDraw.Core.Geometry;
using System;

namespace ConsoleDraw.Core
{
    public static class Renderer
    {
        public static void ResetColor()
        {
            SetColor(ConsoleColor.Black, ConsoleColor.Gray);
        }

        public static void SetColor(ConsoleColor bg, ConsoleColor fg = ConsoleColor.White)
        {
            Console.BackgroundColor = bg;
            Console.ForegroundColor = fg;
        }

        public static Point CursorPosition
        {
            get => new Point(Console.CursorLeft, Console.CursorTop);
            set => Console.SetCursorPosition(value.X, value.Y);
        }
    }
}