using ConsoleDraw.Core.Geometry;
using ConsoleDraw.Core.Interaction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.Core
{

    public class InteractorRenderer
    {
        private readonly ICommand[] _commands;
        private readonly Point _origin;
        private readonly IDictionary<string, CommandRenderer> _commandRenderers;

        public InteractorRenderer(IEnumerable<ICommand> commands, Point origin)
        {
            _commands = commands.Where(com => com.CanRender).ToArray();
            _origin = origin;
            _commandRenderers = _commands.ToDictionary(c => c.Tag, c => new CommandRenderer(c));
            _commands.ForEach(c => c.Activated += Command_Changed);
            _commands.ForEach(c => c.Inactivated += Command_Changed);
        }

        private void Command_Changed(object o, EventArgs e)
        {
            _commandRenderers[((ICommand)o).Tag].Render();
        }

        public void Render()
        {
            Renderer.CursorPosition = _origin;
            GetRenderers()
                .Interleave(RenderSeparator)
                .ForEach(render => render());
        }

        private IEnumerable<Action> GetRenderers()
            => _commandRenderers.Values.Select(cr => (Action)cr.Render);

        private static void RenderSeparator()
        {
            Renderer.ResetColor();
            Console.Write(" | ");
        }
    }
}