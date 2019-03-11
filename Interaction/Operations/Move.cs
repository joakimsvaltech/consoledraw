using ConsoleDraw.Core;
using ConsoleDraw.Core.Geometry;
using ConsoleDraw.Core.Interaction;

namespace ConsoleDraw.Interaction.Operations
{
    public class Move : IExecutable, IModus
    {
        private readonly Canvas _grid;

        public Direction Direction { get; }
        public Move(Canvas grid, Direction direction)
        {
            _grid = grid;
            Direction = direction;
        }

        public bool Execute() => _grid.Step(Direction);
    }
}