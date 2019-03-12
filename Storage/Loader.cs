using ConsoleDraw.Core.Storage;
using System.Drawing;

namespace Storage
{
    public class Loader : ILoader
    {
        public IImage LoadFile(string filename)
        {
            Image img = Image.FromFile(filename);
            return new ConsoleImage(img);
        }
    }
}