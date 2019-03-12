using ConsoleDraw.Core.Interaction;
using System;
using System.Linq;

namespace ConsoleDraw.Core
{
    public abstract class Command : ICommand
    {
        public event EventHandler<EventArgs> StatusChanged;

        protected Command(Canvas grid, string label)
            : this(grid, GetKey(label), GetTag(label), GetName(label), GetModifier(label)) {
        }

        protected Command(Canvas grid, ConsoleKey key = ConsoleKey.NoName, string tag = "", string name = "", ConsoleModifiers modifiers = default)
            => (Grid ,Key, Tag, Name, Modifiers) = (grid, key, Modify(tag, modifiers), name, modifiers);

        public string Tag { get; }
        public string Name { get; }
        public ConsoleKey Key { get; }
        public ConsoleModifiers Modifiers { get; }
        protected Canvas Grid { get; }

        public bool CanRender => !string.IsNullOrEmpty(Tag);

        protected void OnStatusChanged()
        {
            StatusChanged?.Invoke(this, new EventArgs());
        }

        public virtual bool IsActive => false;
        public virtual bool IsEnabled => true;

        public virtual ConsoleColor? NameBackground => null;
        public virtual ConsoleColor? NameForeground => null;

        private static string GetName(string label) => label.Split('-').Last().Replace("_", "");
        private static ConsoleKey GetKey(string label) => Enum.Parse<ConsoleKey>(GetTag(label));
        private static string Modify(string tag, ConsoleModifiers modifiers) =>
            modifiers == default ? tag : $"{modifiers}-{tag}";
        private static string GetTag(string label) => $"{char.ToUpper(label[label.IndexOf('_') + 1])}";
        private static ConsoleModifiers GetModifier(string label) => label.Contains("S-") ? ConsoleModifiers.Shift : default;

        public virtual IExecutable CreateOperation() => throw new NotImplementedException();
    }
}