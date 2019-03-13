using ConsoleDraw.Core.Geometry;
using ConsoleDraw.Core.Interaction;
using ConsoleDraw.Core.Storage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.Core
{
    public class Canvas : IImage
    {
        public event EventHandler<EventArgs<IExecutable>> CommandExecuting;
        public event EventHandler<EventArgs<IExecutable>> CommandExecuted;
        public event EventHandler<EventArgs<ConsoleColor>> ColorChanged;
        public event EventHandler<EventArgs<Cell[]>> CellsChanged;
        public event EventHandler<EventArgs<Cell>> CellChanged;
        public event EventHandler<EventArgs<Point>> CurrentPositionChanged;


        private readonly Cell[,] _cells;
        private Point _current;
        private ConsoleColor _selectedColor = (ConsoleColor)1;
        public readonly Highlight Highlight = new Highlight();
        public Canvas(Point size, int colorCount)
        {
            Size = size;
            _cells = CreateCells(Size.Diagonal(1));
            ColorCount = colorCount;
        }

        public IEnumerable<Cell> Cells => _cells
                   .OfType<Cell>()
                   .Where(c => c.Pos.X < Size.X && c.Pos.Y < Size.Y)
                   .Select(c => c.Clone());

        public Point Size { get; }

        public Point Origin { get; private set; }
        public int ColorCount { get; }
        public IEnumerable<ConsoleColor> Colors => Enumerable.Range(1, ColorCount).Select(index => (ConsoleColor)(index % 16));
        public IEnumerable<Point> Positions => Points(Size);

        public Cell this[Point pos]
        {
            get => _cells[pos.X, pos.Y];
            set => _cells[pos.X, pos.Y] = value;
        }

        public void Fill(IShape shape)
        {
            OnCellsChanged(shape.Area.Select(pos => Paint(pos, SelectedColor)));
        }

        public void Paint(IEnumerable<Cell> cells)
        {
            OnCellsChanged(cells.Select(c => Paint(c.Pos, c.Color)));
        }

        public void Annotate(IEnumerable<Cell> cells)
        {
            cells.ForEach(c => this[c.Pos].Tag = c.Tag);
            OnCellsChanged(cells);
        }

        public Cell CurrentCell => this[CurrentPos];

        public void Execute(ICommand command)
        {
            Perform(command.CreateOperation());
        }

        public Point CurrentPos
        {
            get => _current;
            set
            {
                if (_current == value)
                    return;
                _current = value;
                OnCurrentPositionChanged();
            }
        }

        public ConsoleColor SelectedColor
        {
            get => _selectedColor;
            set
            {
                if (_selectedColor == value)
                    return;
                _selectedColor = value;
                ColorChanged?.Invoke(this, _selectedColor);
            }
        }

        public Point NextPosition(Direction dir) => (Size + CurrentPos.Neighbour(dir)) % Size;

        public bool Step(Direction dir)
        {
            CurrentPos = NextPosition(dir);
            return true;
        }

        public void Plot(Cell cell) => Plot(cell.Pos, cell.Color);

        public void Plot()
        {
            Plot(CurrentPos, SelectedColor);
        }

        private void Plot(Point pos, ConsoleColor color)
        {
            Paint(pos, color);
            OnCellChanged(this[pos]);
        }

        private Cell Paint(Point pos, ConsoleColor color)
        {
            var cell = this[pos];
            cell.Color = color;
            return cell;
        }

        private void Perform(IExecutable operation)
        {
            CommandExecuting?.Invoke(this, new EventArgs<IExecutable>(operation));
            if (operation.Execute())
                CommandExecuted?.Invoke(this, new EventArgs<IExecutable>(operation));
        }

        private static Cell[,] CreateCells(Point size)
        {
            var cells = new Cell[size.X, size.Y];
            foreach (var pos in Points(size))
                cells[pos.X, pos.Y] = new Cell
                {
                    Pos = pos,
                    Color = ConsoleColor.DarkGray
                };
            return cells;
        }

        private static IEnumerable<Point> Points(Point size)
        {
            for (int x = 0; x < size.X; x++)
                for (int y = 0; y < size.Y; y++)
                    yield return (x, y);
        }

        private void OnCellChanged(Cell cell)
        {
            CellChanged?.Invoke(this, cell);
        }

        private void OnCellsChanged(IEnumerable<Cell> cells)
        {
            CellsChanged?.Invoke(this, cells.ToArray());
        }

        private void OnCurrentPositionChanged()
        {
            CurrentPositionChanged?.Invoke(this, CurrentPos);
        }
    }
}