namespace ConsoleDraw.Core
{
    public class UndoCommand : CommandBase
    {
        internal UndoCommand() : base("_Undo") {}

        public override IOperation CreateOperation(Grid grid) => new UndoOperation(grid);

        private class UndoOperation : Operation
        {
            public UndoOperation(Grid grid) : base(grid) { }
            public override void Execute() => Grid.Undo();
        }
    }
}