using System;

namespace ConsoleDraw.Core
{
    public class ActionCommand : CommandBase
    {
        internal ActionCommand(string label, Action execute)
            : this(GetKey(label), execute, GetTag(label), GetName(label)) { }

        public ActionCommand(ConsoleKey key, Action execute, string tag, string name)
            : base(key, tag, name)
            => _executer = execute;

        private readonly Action _executer;

        public override IOperation CreateOperation()
            => new ActionOperation(_executer);

        private class ActionOperation : IOperation
        {
            private readonly Action _executer;

            public ActionOperation(Action executer) => _executer = executer;

            public bool CanUndo => false;

            public void Execute() => _executer();

            public void Undo()
            {
                throw new NotImplementedException();
            }
        }
    }
}