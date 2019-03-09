using ConsoleDraw.Core.Commands.Operations;
using System;

namespace ConsoleDraw.Core
{
    public abstract class CommandBase : ICommand
    {
        public event EventHandler<EventArgs> Activated;
        public event EventHandler<EventArgs> Inactivated;

        public string Tag { get; }
        public string Name { get; }

        protected CommandBase(string label)
            : this(GetKey(label), GetTag(label), GetName(label)) {
        }

        protected CommandBase(ConsoleKey key = ConsoleKey.NoName, string tag = "", string name = "", ConsoleModifiers modifiers = default)
            => (Key, Tag, Name, Modifiers) = (key, tag, name, modifiers);

        public ConsoleKey Key { get; }
        public ConsoleModifiers Modifiers { get; }
        public bool CanRender => !string.IsNullOrEmpty(Tag);

        protected void OnActivated()
        {
            Activated?.Invoke(this, new EventArgs());
        }

        protected void OnInactivated()
        {
            Inactivated?.Invoke(this, new EventArgs());
        }

        public virtual bool IsActive => false;
        public virtual bool IsDisabled => false;

        public virtual ConsoleColor NameBackground => CommandRenderer.DefaultBackground;
        public virtual ConsoleColor NameForeground => CommandRenderer.DefaultForeground;

        protected static string GetName(string label) => label.Replace("_", "");
        protected static ConsoleKey GetKey(string label) => Enum.Parse<ConsoleKey>(GetTag(label));
        protected static string GetTag(string label) => $"{char.ToUpper(label[label.IndexOf('_') + 1])}";

        public virtual IExecutable CreateOperation(Grid grid) => throw new NotImplementedException();
    }
}