using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.Core
{
    public static class CommandRenderer
    {
        public static void RenderCommands(this Interactor interactor)
        {
            Renderer.PositionCursor(interactor.Origin);
            interactor.GetCommandRenderers()
                .Interleave(RenderSeparator)
                .ForEach(render => render());
        }

        private static IEnumerable<Action> GetCommandRenderers(this Interactor interactor)
            => interactor.Commands.Where(c => c.RenderName != null).Select(command => (Action)(() => Render(command)));

        private static void Render(Command command)
        {
            if (command.IsActive())
                Renderer.SetColor(ConsoleColor.White, ConsoleColor.Black);
            else
                Renderer.ResetColor();
            Console.Write(command.Tag);
            command.RenderName!();
        }

        private static void RenderSeparator()
        {
            Renderer.ResetColor();
            Console.Write(" | ");
        }
    }
}