using ConsoleDraw.Core;
using ConsoleDraw.Core.Geometry;
using ConsoleDraw.Core.Interaction;
using System;

namespace ConsoleDraw.Interaction.Commands
{

    public class Move : Command
    {
        private readonly Direction _direction;

        public Move(Canvas grid, ConsoleKey key, Direction direction) : base(grid, key) => _direction = direction;

        public override IExecutable CreateOperation() => new Operations.Move(Grid, _direction);
    }
}