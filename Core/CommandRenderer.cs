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