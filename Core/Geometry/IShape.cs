namespace ConsoleDraw.Core.Geometry
{
    public interface IShape
    {
        Point[] Area { get; }
        Point[] Outline { get; }
        void Update(Point point);
    }
}