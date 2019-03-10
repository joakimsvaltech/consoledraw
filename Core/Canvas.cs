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
        

        private static readonly Random Rand = new Random((int)DateTime.Now.Ticks);
        private readonly Cell[,] _cells;
        private Point _current;
        private ConsoleColor _selectedColor = (ConsoleColor)1;
        public readonly Highlight Highlight = new Highlight();

        public IEnumerable<Cell> Cells => _cells.OfType<Cell>();
        public Point Size { get; }
        public Point Origin { get; private set; }
        public int ColorCount { get; }
        public IEnumerable<ConsoleColor> Colors => Enumerable.Range(1, ColorCount).Select(index => (ConsoleColor)index);
        public IEnumerable<Point> Positions => Points(Size);

        public Canvas(Point size, int colorCount)
            => (Size, ColorCount, _cells)
            = (size, colorCount, CreateCells(size.Diagonal(1)));

        public Cell this[Point pos]
        {
            get => _cells[pos.X, pos.Y];
            set => _cells[pos.X, pos.Y] = value;
        }

        public void FillShape(IShape shape)
        {
            OnCellsChanged(shape.Area.Select(pos => Paint(pos, SelectedColor)));
        }

        public Cell[] GetShadow(IShape shape) => shape.Area.Select(p => this[p]).Select(c => c.Clone()).ToArray();

        public void RandomFill()
        {
            Positions.ForEach(pos => this[pos] = GenerateCell(pos, ColorCount));
        }

        public void Annotate(IEnumerable<Cell> area)
        {
            area.ForEach(cell => this[cell.Pos].Tag = cell.Tag);
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

        public void Fill()
        {
            var area = GetArea(CurrentPos);
            area.ForEach(cell => cell.Color = SelectedColor);
            OnCellsChanged(area);
        }

        public IEnumerable<Cell> GetArea(Point center)
        {
            var next = this[center];
            var color = next.Color;
            var area = new HashSet<Cell> { next };
            var neighbours = new Stack<Cell>(next.Neighbours(this).Where(n => n.Color == color));
            while (neighbours.Any())
            {
                next = neighbours.Pop();
                area.Add(next);
                next.Neighbours(this).Where(n => n.Color == color).Except(area).ForEach(n => neighbours.Push(n));
            }
            return area;
        }

        public void Plot(Cell cell) => Plot(cell.Pos, cell.Color);

        public void Plot()
        {
            Plot(CurrentPos, SelectedColor);
        }

        public void Plot(Point pos, ConsoleColor color)
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
                Color = (ConsoleColor)(Rand.Next(colors) + 1)
            };

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