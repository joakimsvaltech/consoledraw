using ConsoleDraw.Core.Geometry;
using ConsoleDraw.Core.Interaction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.Core
{
    public class CommandRenderer
    {
        public const ConsoleColor DefaultBackground = ConsoleColor.Black;
        public const ConsoleColor ActiveBackground = ConsoleColor.White;
        public const ConsoleColor DefaultForeground = ConsoleColor.Gray;
        public const ConsoleColor ActiveForeground = ConsoleColor.Black;
        public const ConsoleColor DisabledForeground = ConsoleColor.DarkGray;
        private readonly ICommand[] _commands;
        private readonly Point _origin;

        public CommandRenderer(IEnumerable<ICommand> commands, Point origin)
        {
            _commands = commands.ToArray();
            _origin = origin;
        }

        public void Render()
        {
            Renderer.PositionCursor(_origin);
            GetRenderers()
                .Interleave(RenderSeparator)
                .ForEach(render => render());
        }

        private IEnumerable<Action> GetRenderers()
            => _commands
            .Where(com => com.CanRender)
            .Select(com => (Action)(() => Render(com)));

        private static void Render(ICommand command)
        {
            Renderer.SetColor(
                command.IsActive ? ActiveBackground : DefaultBackground,
                command.IsActive ? ActiveForeground : command.IsDisabled ? DisabledForeground : DefaultForeground);
            Console.Write(command.Tag);
            Renderer.SetColor(
                DefaultBackground,
                command.IsDisabled ? DisabledForeground : DefaultForeground);
            Console.Write(". ");
            Renderer.SetColor(
                command.IsDisabled ? DefaultBackground : command.NameBackground ?? DefaultBackground,
                command.IsDisabled ? DisabledForeground : command.NameForeground ?? DefaultForeground);
            Console.Write(command.Name);
        }

        private static void RenderSeparator()
        {
            Renderer.ResetColor();
            Console.Write(" | ");
        }
    }
}