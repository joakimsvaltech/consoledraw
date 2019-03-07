using System;

namespace ConsoleDraw.Core
{
    internal class Command
    {
        internal Command(string label, Action execute, Func<bool>? isActive = null)
            : this(GetKey(label), execute, GetTag(label), () => RenderLabel(label), isActive) { }

        public Command(ConsoleKey key, Action execute, string? tag = null, Action? render = null, Func<bool>? isActive = null)
            => (Tag, Key, Execute, RenderName, IsActive) = (tag, key, execute, render, isActive ?? (() => false));

        public ConsoleKey Key { get; }
        public string? Tag { get; }
        public Action Execute { get; }
        public Action? RenderName { get; }
        public Func<bool> IsActive { get; }

        private static void RenderLabel(string label)
        {
            var name = label.Replace("_", "");
            Renderer.ResetColor();
            Console.Write($". {name}");
        }

        private static ConsoleKey GetKey(string label) => Enum.Parse<ConsoleKey>(GetTag(label));
        private static string GetTag(string label) => $"{char.ToUpper(label[label.IndexOf('_') + 1])}";
    }
}