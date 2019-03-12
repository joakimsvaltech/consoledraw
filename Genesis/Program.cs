﻿using ConsoleDraw.Core;
using ConsoleDraw.Rendering;
using ConsoleDraw.Core.Geometry;
using System;
using System.Runtime.InteropServices;
using ConsoleDraw.Interaction;
using Storage;

namespace ConsoleDraw.Genesis
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
            Console.WriteLine("CONSOLE DRAW - 'Where your imagination becomes reality'");
            Console.WriteLine("------------------------------------------------");
            do
            {
                Run();
                Renderer.ResetColor();
                Console.WriteLine("Press (A) to run again or any other key to quit");
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.A)
                    continue;
                else break;
            }
            while (true);
        }

        static void Run()
        {
            var canvas = CreateCanvas();
            var origin = new Point(Console.CursorLeft, Console.CursorTop);
            var interactorOrigin = origin * (canvas.Size.Y + 1);
            var interactor = CreateInteractor();
            Render();
            while (interactor.Interact()) { }
            Renderer.CursorPosition = origin;

            void Render()
            {
                new CanvasRenderer(canvas, origin).Render();
                new InteractorRenderer(interactor.Commands, interactorOrigin).Render();
            }

            Interactor CreateInteractor()
            {
                var loader = new Loader();
                var input = new Input(interactorOrigin * 2);
                return new Interactor(canvas, loader, input);
            }
        }

        private static Canvas CreateCanvas()
        {
            var cols = Input("number of columns", 10, 200);
            var rows = Input("number of rows", 10, 50);
            var colors = Input("number of colours", 2, 16);
            Console.WriteLine($"Generating grid with {cols} columns, {rows} rows and {colors} colors");
            return new Canvas(new Point(cols, rows), colors);
        }

        private static int Input(string value, int defaultValue = 0, int maxValue = int.MaxValue)
        {
            do
            {
                Console.WriteLine($"Enter {value} (default: {defaultValue})");
                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                    return defaultValue;
                else if (!int.TryParse(input, out var val))
                    Console.WriteLine("Value is not a number!");
                else if (val < 0)
                    Console.WriteLine("Value is too low!");
                else if (val > maxValue)
                    Console.WriteLine("Value is too high!");
                else return val;
            }
            while (true);
        }
    }
}