using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.Core.Events
{
    public class CellsEventArgs : EventArgs
    {
        public CellsEventArgs(IEnumerable<Cell> cells) => Cells = cells.ToArray();
        public Cell[] Cells { get; }
    }
}