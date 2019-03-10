using System;

namespace ConsoleDraw.Core.Events
{
    public class ColorEventArgs : EventArgs
    {
        public ColorEventArgs(ConsoleColor color) => Color = color;
        public ConsoleColor Color { get; }
    }
}