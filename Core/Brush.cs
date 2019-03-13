using System;

namespace ConsoleDraw.Core
{
    public struct Brush : IEquatable<Brush>
    {
        public Brush(ConsoleColor bg, ConsoleColor fg, char shape = 'O')
            => (Background, Foreground, Shape) = (bg, fg, shape);

        public ConsoleColor Background { get; }
        public ConsoleColor Foreground { get; }
        public char Shape { get; }
        public static bool operator ==(Brush left, Brush right)
            => left.Equals(right);

        public static bool operator !=(Brush left, Brush right)
            => !left.Equals(right);

        public static implicit operator Brush(ConsoleColor c)
            => new Brush(c, c, ' ');

        public static implicit operator Brush((ConsoleColor bg, ConsoleColor fg) b)
            => new Brush(b.bg, b.fg, ' ');

        public static implicit operator Brush((ConsoleColor bg, ConsoleColor fg, char shape) b)
            => new Brush(b.bg, b.fg, b.shape);

        public bool Equals(Brush other)
            => other.Background == Background
            && other.Foreground == Foreground
            && other.Shape == Shape;

        public override bool Equals(object obj)
            => obj is Brush b && Equals(b);

        public override int GetHashCode() => Background.GetHashCode() + Foreground.GetHashCode() + Shape.GetHashCode();

        public override string ToString() => $"{Shape}{Background}";
    }
}