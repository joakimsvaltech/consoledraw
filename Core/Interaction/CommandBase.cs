using ConsoleDraw.Core.Commands.Operations;
using System;

namespace ConsoleDraw.Core
{
    public abstract class CommandBase : ICommand
    {
        public event EventHandler<EventArgs> Activated;
        public event EventHandler<EventArgs> Inactivated;

        protected CommandBase(Canvas grid, string label)
            : this(grid, GetKey(label), GetTag(label), GetName(label)) {
        }

        protected CommandBase(Canvas grid, ConsoleKey key = ConsoleKey.NoName, string tag = "", string name = "", ConsoleModifiers modifiers = default)
            => (Grid ,Key, Tag, Name, Modifiers) = (grid, key, tag, name, modifiers);

        public string Tag { get; }
        public string Name { get; }
        public ConsoleKey Key { get; }
        public ConsoleModifiers Modifiers { get; }
        protected Canvas Grid { get; }

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

        public virtual IExecutable CreateOperation() => throw new NotImplementedException();
    }
}