using System;

namespace ConsoleDraw.Genesis
{
    public class Command
    {
        public Command(ConsoleKey key, Action execute, Action? render = null)
            => (Key, Execute, Render) = (key, execute, render);

        public Command(string label, Action execute)
            => (Key, Execute, Render) = (GetKey(label), execute, () => RenderLabel(label));

        private void RenderLabel(string label)
        {
            Renderer.ResetColor();
            var tag = GetTag(label);
            var name = label.Replace("_", "");
            Console.Write($"{tag}. {name}");
        }

        private ConsoleKey GetKey(string label) => Enum.Parse<ConsoleKey>(GetTag(label));
        private string GetTag(string label) => $"{char.ToUpper(label[label.IndexOf('_') + 1])}";

        public ConsoleKey Key { get; }
        public Action Execute { get; }
        public Action? Render { get; }
    }
}