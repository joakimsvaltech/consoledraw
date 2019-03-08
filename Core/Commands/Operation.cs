using System;

namespace ConsoleDraw.Core
{
    public abstract class Operation : IOperation
    {
        protected readonly Grid Grid;
        protected Operation(Grid grid) => Grid = grid;
        public virtual bool CanApply => false;
        public virtual bool CanUndo => false;
        public abstract void Execute();
        public virtual void Apply() => throw new NotImplementedException();
        public virtual void Undo() => throw new NotImplementedException();
    }
}