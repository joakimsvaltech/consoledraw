using System;

namespace FloodFill
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
    }
}