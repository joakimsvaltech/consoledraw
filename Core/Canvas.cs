using ConsoleDraw.Core.Events;
using ConsoleDraw.Core.Geometry;
using ConsoleDraw.Core.Interaction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.Core
{
    public class Canvas
    {
        public event EventHandler<OperationEventArgs> CommandExecuting;
        public event EventHandler<OperationEventArgs> CommandExecuted;
        public event EventHandler<ColorEventArgs> ColorChanged;
        public event EventHandler<CellsEventArgs> CellsChanged;
        public event EventHandler<CellEventArgs> CellChanged;
        public event EventHandler<PointEventArgs> CurrentPositionChanged;
        

        private readonly Cell[,] _cells;
        private Point _current;
        private ConsoleColor _selectedColor = (ConsoleColor)1;
        public readonly Highlight Highlight = new Highlight();

        public IEnumerable<Cell> Cells => _cells
            .OfType<Cell>()
            .Where(c => c.Pos.X < Size.X && c.Pos.Y < Size.Y)
            .Select(c => c.Clone());

        public Point Size { get; }
        public Point Origin { get; private set; }
        public int ColorCount { get; }
        public IEnumerable<ConsoleColor> Colors => Enumerable.Range(1, ColorCount).Select(index => (ConsoleColor)index);
        public IEnumerable<Point> Positions => Points(Size);

        public Canvas(Point size, int colorCount)
        {
            Size = size;
            ColorCount = colorCount;
            _cells = CreateCells(size.Diagonal(1));
        }

        public Cell this[Point pos]
        {
            get => _cells[pos.X, pos.Y];
            set => _cells[pos.X, pos.Y] = value;
        }

        public void FillShape(IShape shape)
        {
            OnCellsChanged(shape.Area.Select(pos => Paint(pos, SelectedColor)));
        }

        public void Paint(IEnumerable<Cell> cells)
        {
            cells.ForEach(c => Paint(c.Pos, c.Color));
            OnCellsChanged(cells);
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
                ColorChanged?.Invoke(this, new ColorEventArgs(_selectedColor));
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
            CommandExecuting?.Invoke(this, new OperationEventArgs(operation));
            if (operation.Execute())
                CommandExecuted?.Invoke(this, new OperationEventArgs(operation));
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
                    yield return new Point(x, y);
        }

        private void OnCellChanged(Cell cell)
        {
            CellChanged?.Invoke(this, new CellEventArgs(cell));
        }

        private void OnCellsChanged(IEnumerable<Cell> cells)
        {
            CellsChanged?.Invoke(this, new CellsEventArgs(cells));
        }

        private void OnCurrentPositionChanged()
        {
            CurrentPositionChanged?.Invoke(this, new PointEventArgs(CurrentPos));
        }
    }
}