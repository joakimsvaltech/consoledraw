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


        public override IOperation CreateOperation(Grid grid) => new ModeOperation(grid, _executer);

        private class ModeOperation : Operation
        {
            private readonly Action _executer;
            public ModeOperation(Grid grid, Action executer) : base(grid) => _executer = executer;
            public override void Execute() => _executer();
        }
    }
}