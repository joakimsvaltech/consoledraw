using ConsoleDraw.Core.Interaction;
using System;

namespace ConsoleDraw.Core.Events
{
    public class OperationEventArgs : EventArgs
    {
        public OperationEventArgs(IExecutable op) => Operation = op;
        public IExecutable Operation { get; }
    }
}