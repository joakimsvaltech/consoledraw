using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.Core
{
    public enum GridMode
    {
        None, Drawing, Rectangle, Ellipse
    }

    public class Grid
    {
        public event EventHandler<OperationEventArgs> CommandExecuting;

        private static readonly Random rand = new Random((int)DateTime.Now.Ticks);
        private readonly Cell[,] _cells;
        private Point _current;
        private IShape? _activeShape;

        private GridMode _mode;

        private readonly Stack<IOperation> _operations = new Stack<IOperation>();

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

        public Cell Peek() => this[CurrentPos];

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
            if (Mode != GridMode.Rectangle && Mode != GridMode.Ellipse) return;
            _activeShape!.Points.ForEach(pos => Paint(pos));
            this.UpdateMarker();
        }

        internal Cell[]? ActiveShadow => _activeShape?.Points.Select(p => this[p]).Select(c => c.Clone()).ToArray();

        public void Execute(ICommand command)
        {
            Perform(command.CreateOperation(this));
        }

        internal void ApplyOperation()
        {
            if (_operations.TryPeek(out var lastOperation) && lastOperation.CanApply)
                lastOperation.Apply();
        }

        internal void Undo()
        {
            if (_operations.Any())
                _operations.Pop().Undo();
        }

        public IEnumerable<Cell> Row(int rowIndex)
                => _cells.Row(rowIndex);

        public Cell CurrentCell => this[CurrentPos];

        public Point CurrentPos
        {
            get => _current;
            set
            {
                this.RemoveMarker();
                _current = value;
                if (Mode == GridMode.Drawing)
                    Plot();
                UpdateActiveShape();
                this.UpdateMarker();
            }
        }

        public void ToggleDraw()
        {
            Mode = Mode == GridMode.Drawing ? GridMode.None : GridMode.Drawing;
        }

        public ConsoleColor SelectedColor { get; set; } = (ConsoleColor)1;
        public GridMode Mode
        {
            get => _mode;
            set
            {
                if (value == _mode)
                    return;
                if (_mode == GridMode.Rectangle || _mode == GridMode.Ellipse)
                {
                    this.Unmark(_activeShape);
                    _activeShape = null;
                }
                _mode = value;
                if (_mode == GridMode.Drawing)
                    Plot();
                if (_mode == GridMode.Rectangle || _mode == GridMode.Ellipse)
                {
                    _activeShape = CreateShape();
                    this.Mark(_activeShape);
                }
            }
        }

        public bool IsDrawing => Mode == GridMode.Drawing;

        public Point NextPosition(Direction dir) => (Size + CurrentPos.Neighbour(dir)) % Size;

        public void Step(Direction dir)
        {
            CurrentPos = NextPosition(dir);
        }

        public void Fill()
        {
            var area = this.GetArea(CurrentPos);
            this.Fill(area);
            this.UpdateMarker();
        }

        public void Plot(Cell cell) => Plot(cell.Pos, cell.Color);

        public void Plot(Point? pos = null, ConsoleColor? color = null)
        {
            Paint(pos, color);
            this.UpdateMarker();
        }

        public void Paint(Point? pos = null, ConsoleColor? color = null)
        {
            var cell = this[pos ?? CurrentPos];
            cell.Color = color ?? SelectedColor;
            this.Render(cell);
        }

        private void Perform(IOperation operation)
        {
            OnCommandExecuting(new OperationEventArgs(operation));
            operation.Execute();
            if (operation.CanUndo)
                _operations.Push(operation);
        }

        private void OnCommandExecuting(OperationEventArgs eventArgs)
        {
            CommandExecuting?.Invoke(this, eventArgs);
        }

        private IShape CreateShape() => Mode switch
        {
            GridMode.Rectangle => (IShape)new Rectangle(CurrentPos),
            GridMode.Ellipse => new Ellipse(CurrentPos),
            _ => throw new NotImplementedException($"{Mode} is not a shape")
        };

        private void UpdateActiveShape()
        {
            if (Mode != GridMode.Rectangle && Mode != GridMode.Ellipse)
                return;
            this.Unmark(_activeShape);
            _activeShape!.End = CurrentPos;
            this.Mark(_activeShape);
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