using ConsoleDraw.Core.Commands.Operations;
using System;

namespace ConsoleDraw.Core
{
    public enum Direction { Up, Down, Left, Right}

    public class NavigationCommand : CommandBase
    {
        private readonly Direction _direction;

        public NavigationCommand(Grid grid, ConsoleKey key, Direction direction) : base(grid, key) => _direction = direction;

        public override IExecutable CreateOperation() => new NavigationOperation(Grid, _direction);
    }

    public class NavigationOperation : IExecutable
    {
        private readonly Grid _grid;

        public Direction Direction { get; }
        public NavigationOperation(Grid grid, Direction direction)
        {
            _grid = grid;
            Direction = direction;
        }

        public bool Execute() => _grid.Step(Direction);
    }
}