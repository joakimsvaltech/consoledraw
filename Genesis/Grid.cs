using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.Genesis
{
    public class Grid
    {
        private static readonly Random rand = new Random((int)DateTime.Now.Ticks);
        private readonly Cell[,] _cells;
        private Point _current;

        public IEnumerable<Cell> Cells => _cells.OfType<Cell>();
        public Point Size { get; }
        public Point Origin { get; private set; }
        public int ColorCount { get; }
        public IEnumerable<ConsoleColor> Colors => Enumerable.Range(1, ColorCount).Select(index => (ConsoleColor)index);
        public IEnumerable<Point> Positions => Points(Size);

        public Grid(int cols, int rows, int colorCount)
            => (Size, ColorCount, _cells)
            = (new Point(cols, rows), colorCount, CreateCells(new Point(cols + 1, rows + 1)));

        public Cell this[Point pos]
        {
            get => _cells[pos.X, pos.Y];
            set => _cells[pos.X, pos.Y] = value;
        }

        public void RandomFill()
        {
            Positions.ForEach(pos => this[pos] = GenerateCell(pos, ColorCount));
        }

        public void Annotate(IEnumerable<Cell> area)
        {
            area.ForEach(cell => this[cell.Pos].Tag = cell.Tag);
        }

        public Point SetPosition(int x = 0, int y = 0)
        {
            var originalPos = new Point(Console.CursorLeft, Console.CursorTop);
            CurrentPos = new Point(x, y);
            return originalPos;
        }

        public void Render()
        {
            if (Origin == default)
                Origin = new Point(Console.CursorLeft, Console.CursorTop);
            else
                Console.SetCursorPosition(Origin.X, Origin.Y);
            this.RenderRows();
        }

        public IEnumerable<Cell> Row(int rowIndex)
                => _cells.Row(rowIndex);

        public Cell CurrentCell => this[CurrentPos];

        public Point CurrentPos
        {
            get => _current;
            private set
            {
                this.RemoveMarker();
                _current = value;
                this.UpdateMarker();
            }
        }

        public ConsoleColor SelectedColor { get; set; } = (ConsoleColor)1;

        public void Up()
        {
            CurrentPos = (CurrentPos.Up + Size) % Size;
        }

        public void Down()
        {
            CurrentPos = CurrentPos.Down % Size;
        }

        public void Left()
        {
            CurrentPos = (CurrentPos.Left + Size) % Size;
        }

        public void Right()
        {
            CurrentPos = CurrentPos.Right % Size;
        }

        public void Fill()
        {
            var area = this.GetArea(CurrentPos);
            this.Fill(area);
        }

        public void Plot()
        {
            this.Plot(SelectedColor);
        }

        private static Cell[,] CreateCells(Point size)
        {
            var cells = new Cell[size.X, size.Y];
            foreach (var pos in Points(size))
                cells[pos.X, pos.Y] = new Cell();
            return cells;
        }

        private static IEnumerable<Point> Points(Point size)
        {
            for (int x = 0; x < size.X; x++)
                for (int y = 0; y < size.Y; y++)
                    yield return new Point(x, y);
        }

        private static Cell GenerateCell(Point pos, int colors)
            => new Cell
            {
                Pos = pos,
                ColorIndex = rand.Next(colors) + 1
            };
    }
}