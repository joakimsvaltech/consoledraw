using System;

namespace ConsoleDraw.Core
{
    public abstract class CommandBase : ICommand
    {
        private readonly string _tag;
        private readonly string _name;

        protected CommandBase(string label)
            : this(GetKey(label), GetTag(label), GetName(label)) {
        }

        protected CommandBase(ConsoleKey key = ConsoleKey.NoName, string tag = "", string name = "")
            => (Key, _tag, _name) = (key, tag, name);

        public ConsoleKey Key { get; }
        public bool CanRender => !string.IsNullOrEmpty(_tag);

        public void Render()
        {
            if (!CanRender)
                return;
            Renderer.SetColor(TagBackground, TagForeground);
            Console.Write(_tag);
            Renderer.ResetColor();
            Console.Write(". ");
            Renderer.SetColor(NameBackground, NameForeground);
            Console.Write(_name);
        }

        protected virtual ConsoleColor TagBackground => ConsoleColor.Black;
        protected virtual ConsoleColor TagForeground => ConsoleColor.Gray;
        protected virtual ConsoleColor NameBackground => ConsoleColor.Black;
        protected virtual ConsoleColor NameForeground => ConsoleColor.Gray;

        protected static string GetName(string label) => label.Replace("_", "");
        protected static ConsoleKey GetKey(string label) => Enum.Parse<ConsoleKey>(GetTag(label));
        protected static string GetTag(string label) => $"{char.ToUpper(label[label.IndexOf('_') + 1])}";

        public virtual IOperation CreateOperation() => throw new NotImplementedException();
    }
}