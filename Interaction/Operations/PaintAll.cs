using ConsoleDraw.Interaction.Operations;
using System.Linq;

namespace ConsoleDraw.Core
{
    public abstract class PaintAll : Paint
    {
        protected PaintAll(Canvas grid) : base(grid) { }

        protected override Cell[] GetShadow() => Grid.Cells.ToArray();
    }
}