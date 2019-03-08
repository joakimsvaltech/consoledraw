using System;

namespace ConsoleDraw.Core
{
    public enum Direction { Up, Down, Left, Right}

    public class NavigationCommand : CommandBase
    {
        public NavigationCommand(ConsoleKey key, Direction direction) : base(key) => _direction = direction;
        private readonly Direction _direction;
        public override IOperation CreateOperation(Grid grid) => new NavigationOperation(grid, _direction);
    }

    public class NavigationOperation : Operation
    {
        public Direction Direction { get; }
        public NavigationOperation(Grid grid, Direction direction) : base(grid) => Direction = direction;
        public override void Execute() => Grid.Step(Direction);
    }
}