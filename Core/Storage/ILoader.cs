namespace ConsoleDraw.Core.Storage
{
    public interface IRepository
    {
        IImage Load(string filename);
        void Save(string filename, IImage image);
    }
}