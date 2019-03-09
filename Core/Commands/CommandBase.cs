using ConsoleDraw.Core.Commands.Operations;
using System;

namespace ConsoleDraw.Core
{
    public abstract class CommandBase : ICommand
    {
        private const ConsoleColor DefaultBackground = ConsoleColor.Black;
        private const ConsoleColor ActiveBackground = ConsoleColor.White;
        private const ConsoleColor DefaultForeground = ConsoleColor.Gray;
        private const ConsoleColor ActiveForeground = ConsoleColor.Black;
        private const ConsoleColor DisabledForeground = ConsoleColor.DarkGray;

        private readonly string _tag;
        private readonly string _name;

        protected CommandBase(string label)
            : this(GetKey(label), GetTag(label), GetName(label)) {
        }

        protected CommandBase(ConsoleKey key = ConsoleKey.NoName, string tag = "", string name = "", ConsoleModifiers modifiers = default)
            => (Key, _tag, _name, Modifiers) = (key, tag, name, modifiers);

        public ConsoleKey Key { get; }
        public ConsoleModifiers Modifiers { get; }
        public bool CanRender => !string.IsNullOrEmpty(_tag);

        public void Render()
        {
            if (!CanRender)
                return;
            Renderer.SetColor(
                IsActive ? ActiveBackground : DefaultBackground,
                IsActive ? ActiveForeground : IsDisabled ? DisabledForeground : DefaultForeground);
            Console.Write(_tag);
            Renderer.SetColor(
                DefaultBackground,
                IsDisabled ? DisabledForeground : DefaultForeground);
            Console.Write(". ");
            Renderer.SetColor(
                IsDisabled ? DefaultBackground : NameBackground, 
                IsDisabled ? DisabledForeground : NameForeground);
            Console.Write(_name);
        }

        protected virtual bool IsActive => false;
        protected virtual bool IsDisabled => false;

        protected virtual ConsoleColor NameBackground => DefaultBackground;
        protected virtual ConsoleColor NameForeground => DefaultForeground;

        protected static string GetName(string label) => label.Replace("_", "");
        protected static ConsoleKey GetKey(string label) => Enum.Parse<ConsoleKey>(GetTag(label));
        protected static string GetTag(string label) => $"{char.ToUpper(label[label.IndexOf('_') + 1])}";

        public virtual IExecutable CreateOperation(Grid grid) => throw new NotImplementedException();
    }
}