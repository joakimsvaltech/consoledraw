using ConsoleDraw.Core;
using ConsoleDraw.Core.Interaction;
using ConsoleDraw.Core.Storage;

namespace ConsoleDraw.Interaction.Commands
{
    public class Save : Command
    {
        private readonly IInput _input;
        private readonly IRepository _repository;

        public Save(IInput input, IRepository repository, Canvas grid) : base(grid, "S-_Save")
        {
            _input = input;
            _repository = repository;
        }

        public override IExecutable CreateOperation() => new Operation(_input, _repository, Grid);

        private class Operation : IExecutable
        {
            private readonly IInput _input;
            private readonly IRepository _repository;
            private readonly IImage _image;

            public Operation(IInput input, IRepository loader, IImage image)
            {
                _input = input;
                _repository = loader;
                _image = image;
            }

            public bool Execute()
            {
                var filename = _input.Get("Filename");
                _repository.Save(filename, _image);
                _input.Respond("File saved!");
                return true;
            }
        }
    }
}