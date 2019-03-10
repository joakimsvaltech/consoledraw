using ConsoleDraw.Core.Geometry;
using ConsoleDraw.Core.Interaction;
using System;

namespace ConsoleDraw.Core
{
    internal class CommandRenderer
    {
        private const ConsoleColor DefaultBackground = ConsoleColor.Black;
        private const ConsoleColor ActiveBackground = ConsoleColor.White;
        private const ConsoleColor DefaultForeground = ConsoleColor.Gray;
        private const ConsoleColor ActiveForeground = ConsoleColor.Black;
        private const ConsoleColor DisabledForeground = ConsoleColor.DarkGray;

        private Point? _position;

        public CommandRenderer(ICommand command) => Command = command;

        public ICommand Command { get; }

        public void Render()
        {
            UpdatePosition();
            RenderTag();
            RenderDot();
            RenderName();
        }

        private void UpdatePosition()
        {
            Renderer.CursorPosition = (_position ??= Renderer.CursorPosition).Value;
        }

        private void RenderTag()
        {
            Renderer.SetColor(
                Command.IsActive ? ActiveBackground : DefaultBackground,
                Command.IsActive ? ActiveForeground : Command.IsDisabled ? DisabledForeground : DefaultForeground);
            Console.Write(Command.Tag);
        }

        private void RenderDot()
        {
            Renderer.SetColor(
                DefaultBackground,
                Command.IsDisabled ? DisabledForeground : DefaultForeground);
            Console.Write(". ");
        }

        private void RenderName()
        {
            Renderer.SetColor(
                Command.IsDisabled ? DefaultBackground : Command.NameBackground ?? DefaultBackground,
                Command.IsDisabled ? DisabledForeground : Command.NameForeground ?? DefaultForeground);
            Console.Write(Command.Name);
        }
    }
}