using System;

namespace FloodFill
{
    public static class GridInteractor
    {
        public static bool Interact(this Grid grid)
        {
            var op = GetOperation(grid);
            if (op == Exit)
                return false;
            op();
            return true;
        }

        private static Action GetOperation(this Grid grid)
        {
            var key = Console.ReadKey();
            return key.Key switch
            {
                ConsoleKey.UpArrow => grid.Up,
                ConsoleKey.DownArrow => grid.Down,
                ConsoleKey.LeftArrow => grid.Left,
                ConsoleKey.RightArrow => grid.Right,
                ConsoleKey.X => Exit,
                _ => NoOp
            };
        }

        private static readonly Action Exit = () => { };
        private static readonly Action NoOp = () => { };
    }
}