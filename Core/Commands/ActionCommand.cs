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

        public override IOperation CreateOperation(Grid grid)
            => new ActionOperation(grid, _executer);

        private class ActionOperation : Operation
        {
            private readonly Action _executer;

            public ActionOperation(Grid grid, Action executer) : base(grid) => _executer = executer;

            public override void Execute() => _executer();
        }
    }
}