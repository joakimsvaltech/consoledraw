using System;

namespace ConsoleDraw.Core
{
    public class ModeCommand : CommandBase
    {
        internal ModeCommand(string label, Action execute, Func<bool> isActive)
            : base(label) => (_executer, _isActiveChecker) = (execute, isActive);

        private readonly Action _executer;
        private readonly Func<bool> _isActiveChecker;

        protected override ConsoleColor TagBackground => _isActiveChecker()
            ? ConsoleColor.White
            : base.TagBackground;

        protected override ConsoleColor TagForeground => _isActiveChecker()
            ? ConsoleColor.Black
            : base.TagForeground;


        public override IOperation CreateOperation() => new ModeOperation(_executer);

        private class ModeOperation : IOperation
        {
            private readonly Action _executer;

            public ModeOperation(Action executer) => _executer = executer;

            public bool CanUndo => false;

            public void Execute() => _executer();

            public void Undo()
            {
                throw new NotImplementedException();
            }
        }
    }
}