using System;

namespace ConsoleDraw.Core
{
    public interface ICommand
    {
        ConsoleKey Key { get; }
        IOperation CreateOperation();
        void Render();
        bool CanRender { get; }
    }
}