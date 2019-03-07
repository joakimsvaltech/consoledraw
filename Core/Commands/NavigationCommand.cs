using System;

namespace ConsoleDraw.Core
{
    public class NavigationCommand : CommandBase
    {
        public NavigationCommand(ConsoleKey key, Action execute) : base(key)
            => _executer = execute;

        private readonly Action _executer;

        public override IOperation CreateOperation() => new NavigationOperation(_executer);

        private class NavigationOperation : IOperation
        {
            private readonly Action _executer;

            public NavigationOperation(Action executer) => _executer = executer;

            public bool CanUndo => false;

            public void Execute() => _executer();

            public void Undo()
            {
                throw new NotImplementedException();
            }
        }
    }
}