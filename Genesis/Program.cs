using System;
using System.Runtime.InteropServices;

namespace FloodFill
{
    class Program
    {
        [DllImport("kernel32.dll", ExactSpelling = true)]

        private static extern IntPtr GetConsoleWindow();
        private static readonly IntPtr ThisConsole = GetConsoleWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]

        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int MAXIMIZE = 3;


        static void Main()
        {
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            ShowWindow(ThisConsole, MAXIMIZE);

            Console.WriteLine("Hello FloodFill!");
            do
            {
                Run();
                ResetColors();
                Console.WriteLine("Press (A) to run again or any otehr key to quit");
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.A)
                    continue;
                else break;
            }
            while (true);
        }

        static void Run()
        {
            var cols = Input("number of columns");
            var rows = Input("number of rows");
            var colors = Input("number of colours");
            Console.WriteLine($"Generating grid with {cols} columns, {rows} rows and {colors} colors");
            var grid = new Grid(cols, rows, colors);
            var largestConnectedArea = grid.FindLargestConnectedArea();
            grid.Annotate(largestConnectedArea);
            grid.Render();
            var originalPosition = grid.SetPosition();
            while (grid.Interact()) { }
            Console.SetCursorPosition(originalPosition.X, originalPosition.Y);
        }

        private static void ResetColors()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
        }

        private static int Input(string value)
        {
            Console.WriteLine($"Enter {value}");
            return int.Parse(Console.ReadLine());
        }
    }
}