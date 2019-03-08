using System;

namespace ConsoleDraw.Core
{
    public class EscapeCommand : CommandBase
    {
        public EscapeCommand() : base(ConsoleKey.Escape, "Esc", "Escape") { }
        public override IOperation CreateOperation(Grid grid) => new EscapeOperation(grid);

        private class EscapeOperation : Operation
        {
            public EscapeOperation(Grid grid) : base(grid) { }
            public override void Execute() => Grid.Mode = GridMode.None;
        }
    }
}