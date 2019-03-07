using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.Core
{
    public enum DrawMode {
        None, Drawing, Rectangle, Ellipse
    } 

    public class Grid
    {
        private static readonly Random rand = new Random((int)DateTime.Now.Ticks);
        private readonly Cell[,] _cells;
        private Point _current;
        private IShape? _activeShape;
        private DrawMode _mode;

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
            GridRenderer.Render(this);
        }

        internal void FillShape()
        {
            if (Mode != DrawMode.Rectangle && Mode != DrawMode.Ellipse) return;
            _activeShape!.Points.ForEach(Paint);
            this.UpdateMarker();
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
                if (Mode == DrawMode.Drawing)
                    Plot();
                UpdateActiveShape();
                this.UpdateMarker();
            }
        }

        public void ToggleDraw()
        {
            Mode = Mode == DrawMode.Drawing ? DrawMode.None : DrawMode.Drawing;
        }

        public ConsoleColor SelectedColor { get; set; } = (ConsoleColor)1;
        public DrawMode Mode
        {
            get => _mode;
            set
            {
                if (value == _mode)
                    return;
                if (_mode == DrawMode.Rectangle || _mode == DrawMode.Ellipse)
                {
                    this.Unmark(_activeShape);
                    _activeShape = null;
                }
                _mode = value;
                if (_mode == DrawMode.Drawing)
                    Plot();
                if (_mode == DrawMode.Rectangle || _mode == DrawMode.Ellipse)
                {
                    _activeShape = CreateShape();
                    this.Mark(_activeShape);
                }
            }
        }

        private IShape CreateShape() => Mode switch
        {
            DrawMode.Rectangle => (IShape)new Rectangle(CurrentPos),
            DrawMode.Ellipse => new Ellipse(CurrentPos),
            _ => throw new NotImplementedException($"{Mode} is not a shape")
        };

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
            this.UpdateMarker();
        }

        public void Plot()
        {
            this[CurrentPos].Color = SelectedColor;
            this.Plot(CurrentPos);
            this.UpdateMarker();
        }

        private void UpdateActiveShape()
        {
            if (Mode != DrawMode.Rectangle && Mode != DrawMode.Ellipse)
                return;
            this.Unmark(_activeShape);
            _activeShape!.End = CurrentPos;
            this.Mark(_activeShape);
        }

        private void Paint(Point pos)
        {
            this[pos].Color = SelectedColor;
            this.Plot(pos);
        }

        private static Cell[,] CreateCells(Point size)
        {
            var cells = new Cell[size.X, size.Y];
            foreach (var pos in Points(size))
                cells[pos.X, pos.Y] = new Cell
                {
                    Pos = pos
                };
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
                Color = (ConsoleColor)(rand.Next(colors) + 1)
            };
    }
}