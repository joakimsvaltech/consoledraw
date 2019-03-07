namespace ConsoleDraw.Core
{
    public interface IShape
    {
        Point Start { get; }
        Point End { get; set; }
        Point Size { get; }
        Point[] Points { get; }
        Point[] Outline { get; }
    }
}