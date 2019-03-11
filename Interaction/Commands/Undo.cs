using ConsoleDraw.Core;
using ConsoleDraw.Core.Interaction;
using ConsoleDraw.Interaction.Operations;
using System.Linq;

namespace ConsoleDraw.Interaction.Commands
{
    public class Undo : Revert<IUndoable, Operations.Undo>
    {
        internal Undo(Canvas grid) : base(grid, "_Undo") {}
        public override IExecutable CreateOperation() => new Operations.Undo(this, Grid);
    }
}