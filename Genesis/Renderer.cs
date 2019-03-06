using System;

namespace ConsoleDraw.Genesis
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