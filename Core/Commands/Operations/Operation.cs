using ConsoleDraw.Core.Commands.Operations;

namespace ConsoleDraw.Core
{
    public abstract class Operation : IExecutable
    {
        protected readonly Grid Grid;
        protected Operation(Grid grid) => Grid = grid;
        public abstract bool Execute();
    }
}