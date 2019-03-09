using ConsoleDraw.Core.Commands.Operations;
using System;

namespace ConsoleDraw.Core
{
    public class OperationEventArgs : EventArgs
    {
        public OperationEventArgs(IExecutable op) => Operation = op;
        public IExecutable Operation { get; }
    }
}