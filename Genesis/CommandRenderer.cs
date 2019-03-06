using System;

namespace ConsoleDraw.Genesis
{
    public static class CommandRenderer
    {
        public static void RenderCommands(this Interactor interactor)
        {
            Renderer.PositionCursor(interactor.Origin);
            interactor.CommandRenderers
                .Interleave(RenderSeparator)
                .ForEach(render => render());
        }

        private static void RenderSeparator()
        {
            Renderer.ResetColor();
            Console.Write(" | ");
        }
    }
}