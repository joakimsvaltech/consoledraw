using System;

namespace ConsoleDraw.Core
{
    public interface ICommand
    {
        ConsoleKey Key { get; }
        IOperation CreateOperation(Grid grid);
        void Render();
        bool CanRender { get; }
    }
}