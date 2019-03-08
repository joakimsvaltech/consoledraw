using System.Linq;

namespace ConsoleDraw.Core
{
    public class FillCommand : CommandBase
    {
        internal FillCommand() : base("_Fill") { }
        public override IOperation CreateOperation(Grid grid) => new FillOperation(grid);

        private class FillOperation : UndoableOperation
        {
            private Cell[] _oldCells = new Cell[0];

            public FillOperation(Grid grid) : base(grid) { }

            protected override void DoExecute()
            {
                _oldCells = Grid.GetArea(Grid.CurrentPos).Select(c => c.Clone()).ToArray();
                Grid.Fill();
            }

            protected override void DoUndo()
            {
                _oldCells.ForEach(Grid.Plot);
            }
        }
    }
}