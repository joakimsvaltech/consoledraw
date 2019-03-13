using ConsoleDraw.Core;
using ConsoleDraw.Core.Interaction;
using ConsoleDraw.Core.Storage;
using System.Linq;

namespace ConsoleDraw.Interaction.Commands
{
    public class Load : Command
    {
        private readonly IInput _input;
        private readonly IRepository _repository;

        public Load(IInput input, IRepository repository, Canvas grid) : base(grid, "S-_Load") {
            _input = input;
            _repository = repository;
        }

        public override IExecutable CreateOperation() => new Operation(_input, _repository, Grid);

        private class Operation : PaintAll
        {
            private readonly IInput _input;
            private readonly IRepository _repository;

            public Operation(IInput input, IRepository repository, Canvas grid) : base(grid)
            {
                _input = input;
                _repository = repository;
            }

            protected override void Apply()
            {
                var filename = _input.Get("Filename");
                var image = _repository.Load(filename);
                Canvas.Paint(image.Cells.Where(c => c.Pos < Canvas.Size));
            }
        }
    }
}