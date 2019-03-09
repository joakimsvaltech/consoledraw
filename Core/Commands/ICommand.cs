using ConsoleDraw.Core.Commands.Operations;
using System;

namespace ConsoleDraw.Core
{
    public interface ICommand
    {
        event EventHandler<EventArgs> Activated;
        event EventHandler<EventArgs> Inactivated;
        ConsoleKey Key { get; }
        ConsoleColor NameBackground { get; }
        ConsoleColor NameForeground { get; }
        ConsoleModifiers Modifiers { get; }
        IExecutable CreateOperation(Grid grid);
        bool CanRender { get; }
        bool IsActive { get; }
        bool IsDisabled { get; }
        string Tag { get; }
        string Name { get; }
    }
}