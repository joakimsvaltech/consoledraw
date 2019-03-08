using System;

namespace ConsoleDraw.Core
{
    public class OperationEventArgs : EventArgs
    {
        public OperationEventArgs(IOperation op) => Operation = op;
        public IOperation Operation { get; }
    }
}