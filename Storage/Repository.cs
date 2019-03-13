using ConsoleDraw.Core.Storage;
using System.Drawing;

namespace Storage
{
    public class Repository : IRepository
    {
        public IImage Load(string filename)
        {
            Image img = Image.FromFile(filename);
            return new ConsoleImage(img);
        }

        public void Save(string filename, IImage image)
        {
            var bmp = new ConsoleImage(image).ToBitmap();
            bmp.Save(filename);
        }
    }
}