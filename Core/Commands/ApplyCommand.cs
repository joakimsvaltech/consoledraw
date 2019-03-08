using System;

namespace ConsoleDraw.Core
{
    public class ApplyCommand : CommandBase
    {
        public ApplyCommand()
            : base(ConsoleKey.Enter, "Enter", "Apply") { }

        public override IOperation CreateOperation(Grid grid)
            => new ApplyOperation(grid);

        private class ApplyOperation : Operation
        {
            public ApplyOperation(Grid grid) : base(grid) { }
            public override void Execute() => Grid.ApplyOperation();
        }
    }
}