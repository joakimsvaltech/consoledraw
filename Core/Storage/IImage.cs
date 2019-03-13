using ConsoleDraw.Core.Geometry;
using System.Collections.Generic;

namespace ConsoleDraw.Core.Storage
{
    public interface IImage
    {
        IEnumerable<Cell> Cells { get; }
        Point Size { get; }
    }
}
