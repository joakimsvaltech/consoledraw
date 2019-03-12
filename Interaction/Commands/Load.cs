using ConsoleDraw.Core;
using ConsoleDraw.Core.Interaction;
using ConsoleDraw.Core.Storage;
using System.Linq;

namespace ConsoleDraw.Interaction.Commands
{
    public class Load : Command
    {
        private readonly IInput _input;
        private readonly ILoader _loader;

        public Load(IInput input, ILoader loader, Canvas grid) : base(grid, "S-_Load") {
            _input = input;
            _loader = loader;
        }

        public override IExecutable CreateOperation() => new Operation(_input, _loader, Grid);

        private class Operation : PaintAll
        {
            private readonly IInput _input;
            private readonly ILoader _loader;

            public Operation(IInput input, ILoader loader, Canvas grid) : base(grid)
            {
                _input = input;
                _loader = loader;
            }

            protected override void Apply()
            {
                var filename = _input.Get("Filename");
                var image = _loader.LoadFile(filename);
                Grid.Paint(image.Cells.Where(c => c.Pos < Grid.Size));
            }
        }
    }
}