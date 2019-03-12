using ConsoleDraw.Core.Interaction;
using ConsoleDraw.Interaction.Operations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.Core
{
    public class RandomFill : Command
    {
        private static readonly Random Rand = new Random((int)DateTime.Now.Ticks);
        internal RandomFill(Canvas grid) : base(grid, ConsoleKey.Z, "Z", "Random fill") { }
        public override IExecutable CreateOperation() => new Operation(Grid);

        private class Operation : PaintAll
        {
            public Operation(Canvas grid) : base(grid) { }

            protected override void Apply()
            {
                Grid.Paint(GetRandomCells());
            }

            private IEnumerable<Cell> GetRandomCells()
            {
                var randomizedCells = Grid.Cells.ToArray();
                randomizedCells.ForEach(c => c.Color = (ConsoleColor)(Rand.Next(Grid.ColorCount) + 1));
                return randomizedCells;
            }
        }
    }
}