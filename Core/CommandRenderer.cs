using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.Core
{
    public static class CommandRenderer
    {
        public const ConsoleColor DefaultBackground = ConsoleColor.Black;
        public const ConsoleColor ActiveBackground = ConsoleColor.White;
        public const ConsoleColor DefaultForeground = ConsoleColor.Gray;
        public const ConsoleColor ActiveForeground = ConsoleColor.Black;
        public const ConsoleColor DisabledForeground = ConsoleColor.DarkGray;

        public static void Render(this Interactor interactor)
        {
            Renderer.PositionCursor(interactor.Origin);
            interactor.GetCommandRenderers()
                .Interleave(RenderSeparator)
                .ForEach(render => render());
        }

        public static void Render(this ICommand command)
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
                command.IsDisabled ? DefaultBackground : command.NameBackground,
                command.IsDisabled ? DisabledForeground : command.NameForeground);
            Console.Write(command.Name);
        }

        private static IEnumerable<Action> GetCommandRenderers(this Interactor interactor)
            => interactor.Commands
            .Where(com => com.CanRender)
            .Select(com => (Action)com.Render);

        private static void RenderSeparator()
        {
            Renderer.ResetColor();
            Console.Write(" | ");
        }
    }
}