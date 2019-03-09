using ConsoleDraw.Core.Commands.Operations;
using System;

namespace ConsoleDraw.Core
{
    public interface ICommand
    {
        ConsoleKey Key { get; }
        ConsoleModifiers Modifiers { get; }
        IOperation CreateOperation(Grid grid);
        void Render();
        bool CanRender { get; }
    }
}