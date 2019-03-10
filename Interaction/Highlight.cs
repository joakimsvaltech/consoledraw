using ConsoleDraw.Core;
using ConsoleDraw.Core.Events;
using System;
using System.Drawing;
using System.Linq;

namespace ConsoleDraw.Interaction
{
    public class Highlight
    {
        public event EventHandler<CellsEventArgs> AreaChanged;

        private Cell[] _area = new Cell[0];

        public Cell[] Area
        {
            get => _area;
            set
            {
                if (value.SequenceEqual(_area)) return;
                _area = value;
                AreaChanged?.Invoke(this, new CellsEventArgs(Area));
            }
        }
    }
}