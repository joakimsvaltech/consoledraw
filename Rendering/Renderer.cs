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

        public static void PositionCursor(Point absPos)
        {
            Console.SetCursorPosition(absPos.X, absPos.Y);
        }
    }
}