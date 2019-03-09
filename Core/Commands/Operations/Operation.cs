using ConsoleDraw.Core.Commands.Operations;

namespace ConsoleDraw.Core
{
    public abstract class Operation : IOperation
    {
        protected readonly Grid Grid;
        protected Operation(Grid grid) => Grid = grid;
        public abstract bool Execute();
    }
}