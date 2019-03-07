using System;

namespace ConsoleDraw.Core
{
    public class UndoCommand : CommandBase
    {
        internal UndoCommand(Action execute)
            : base("_Undo") => _execute = execute;

        private readonly Action _execute;

        public override IOperation CreateOperation() => new UndoOperation(_execute);

        private class UndoOperation : IOperation
        {
            private readonly Action _execute;

            public UndoOperation(Action execute) => _execute = execute;

            public bool CanUndo => false;

            public void Execute() => _execute();

            public void Undo()
            {
                throw new NotImplementedException();
            }
        }
    }
}