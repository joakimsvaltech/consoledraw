using ConsoleDraw.Core.Interaction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.Core
{
    public class RandomFillCommand : CommandBase
    {
        private static readonly Random Rand = new Random((int)DateTime.Now.Ticks);
        internal RandomFillCommand(Canvas grid) : base(grid, ConsoleKey.Z, "Z", "Random fill") { }
        public override IExecutable CreateOperation() => new RandomFillOperation(Grid);

        private class RandomFillOperation : PaintOperation
        {
            public RandomFillOperation(Canvas grid) : base(grid) { }

            protected override void Paint()
            {
                Grid.Paint(GetRandomCells());
            }

            protected override Cell[] GetShadow() => Grid.Cells.ToArray();

            private IEnumerable<Cell> GetRandomCells()
            {
                var randomizedCells = Grid.Cells.ToArray();
                randomizedCells.ForEach(c => c.Color = (ConsoleColor)(Rand.Next(Grid.ColorCount) + 1));
                return randomizedCells;
            }
        }
    }
}