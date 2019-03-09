using System;

namespace ConsoleDraw.Core
{
    public class ColorEventArgs : EventArgs
    {
        public ColorEventArgs(ConsoleColor color) => Color = color;
        public ConsoleColor Color { get; }
    }
}