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
            => grid.GetOperation(Console.ReadKey(true));

        private static Action GetOperation(this Grid grid, ConsoleKeyInfo keyInfo)
            => keyInfo.Key switch
        {
            ConsoleKey.UpArrow => grid.Up,
            ConsoleKey.DownArrow => grid.Down,
            ConsoleKey.LeftArrow => grid.Left,
            ConsoleKey.RightArrow => grid.Right,
            ConsoleKey.P => grid.Plot,
            ConsoleKey.F => grid.Fill,
            ConsoleKey.X => Exit,
            ConsoleKey key when IsNumberKey(key) => grid.GetNumberKeyOperation(key),
            _ => NoOp
        };

        private static Action GetNumberKeyOperation(this Grid grid, ConsoleKey key)
         => () => grid.SetColor((ConsoleColor)((int)key - (int)ConsoleKey.D0));

        private static bool IsNumberKey(ConsoleKey key) => key > ConsoleKey.D0 && key <= ConsoleKey.D9;

        private static readonly Action Exit = () => { };
        private static readonly Action NoOp = () => { };
    }
}