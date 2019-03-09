using ConsoleDraw.Core.Commands.Operations;
using System;

namespace ConsoleDraw.Core
{
    public abstract class Operation : IOperation
    {
        protected readonly Grid Grid;
        protected Operation(Grid grid) => Grid = grid;
        public virtual bool CanApply => false;
        public virtual bool CanUndo => false;
        public abstract bool Execute();
        public virtual bool Apply() => throw new NotImplementedException();
        public virtual bool Undo() => throw new NotImplementedException();
        public virtual bool Redo() => throw new NotImplementedException();
    }
}