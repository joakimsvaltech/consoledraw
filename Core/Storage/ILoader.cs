namespace ConsoleDraw.Core.Storage
{
    public interface ILoader
    {
        IImage LoadFile(string filename);
    }
}