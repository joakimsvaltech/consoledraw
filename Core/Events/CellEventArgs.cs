using System;

namespace ConsoleDraw.Core.Events
{
    public class CellEventArgs : EventArgs
    {
        public CellEventArgs(Cell cell) => Cell = cell;
        public Cell Cell { get; }
    }
}