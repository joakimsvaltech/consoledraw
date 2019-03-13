using ConsoleDraw.Core;
using ConsoleDraw.Core.Geometry;
using ConsoleDraw.Interaction;
using System;

namespace ConsoleDraw.Genesis
{
    internal class Input : IInput
    {
        private readonly Point _inputOrigin;

        public Input(Point inputOrigin)
        {
            _inputOrigin = inputOrigin;
        }

        public string Get(string name)
        {
            Renderer.ResetColor();
            Renderer.CursorPosition = _inputOrigin;
            ClearLine();
            ClearLine();
            Renderer.CursorPosition = _inputOrigin;
            Console.WriteLine($"Input {name}:");
            return Console.ReadLine();
        }

        public void Respond(string message)
        {
            Console.WriteLine(message);
        }

        private void ClearLine()
        {
            Console.WriteLine("                                                                                               ");
        }
    }
}