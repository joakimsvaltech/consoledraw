using System;
using System.Collections.Generic;
using System.Linq;

namespace FloodFill
{
    public class Grid
    {
        private static readonly Random rand = new Random((int)DateTime.Now.Ticks);
        private readonly Cell[,] _cells;
        private Point Origin, _current;

        public Point Size { get; }
        public int Colors { get; }

        private Point Current
        {
            get => _current;
            set
            {
                RemoveMarker();
                _current = value;
                UpdateMarker();
            }
        }

        public Cell this[Point pos] => _cells[pos.X, pos.Y];

        public Grid(int cols, int rows, int colors)
        {
            Size = new Point(cols, rows);
            Colors = colors;
            _cells = new Cell[Size.X + 1, Size.Y + 1];
            for (int x = 0; x < Size.X; x++)
                for (int y = 0; y < Size.Y; y++)
                    _cells[x, y] = GenerateCell(x, y, colors);
            for (int x = 0; x < Size.X; x++)
                _cells[x, Size.Y] = new Cell();
            for (int y = 0; y < Size.Y; y++)
                _cells[Size.X, y] = new Cell();
        }

        public void Annotate(IEnumerable<Cell> area)
        {
            foreach (var cell in area)
                this[cell.Pos].Tag = cell.Tag;
        }

        public Point SetPosition(int x = 0, int y = 0)
        {
            var originalPos = new Point(Console.CursorLeft, Console.CursorTop);
            Current = new Point(x, y);
            return originalPos;
        }

        public bool Interact()
        {
            var op = GetOperation();
            if (op == Exit)
                return false;
            op();
            return true;
        }

        private Action GetOperation()
        {
            var key = Console.ReadKey();
            return key.Key switch
            {
                ConsoleKey.UpArrow => Up,
                ConsoleKey.DownArrow => Down,
                ConsoleKey.LeftArrow => Left,
                ConsoleKey.RightArrow => Right,
                ConsoleKey.X => Exit,
                _ => NoOp
            };
        }

        private readonly Action Exit = () => { };
        private readonly Action NoOp = () => { };

        private void Up()
        {
            Current = (Current.Up + Size) % Size;
        }

        private void Down()
        {
            Current = Current.Down % Size;
        }

        private void Left()
        {
            Current = (Current.Left + Size) % Size;
        }

        private void Right()
        {
            Current = Current.Right % Size;
        }

        public void UpdateMarker()
        {
            PositionCursor(Origin + Current);
            MarkGridCell(this[Current]);
        }

        public void RemoveMarker()
        {
            Plot(Current);
            Plot(Current.Right);
        }

        public void Render()
        {
            if (Origin == default)
                Origin = new Point(Console.CursorLeft, Console.CursorTop);
            else
                Console.SetCursorPosition(Origin.X, Origin.Y);
            for (int y = 0; y < Size.Y; y++)
                PrintGridLine(_cells.Row(y));
        }

        private void Plot(Point pos)
        {
            PositionCursor(Origin + pos);
            PrintGridCell(this[pos]);
        }

        private void PositionCursor(Point absPos)
        {
            Console.SetCursorPosition(absPos.X, absPos.Y);
        }

        private static Cell GenerateCell(int x, int y, int colors)
            => new Cell
            {
                Pos = new Point(x, y),
                ColorIndex = rand.Next(colors)
            };

        private static void PrintGridLine(IEnumerable<Cell> row)
        {
            foreach (var cell in row)
                PrintGridCell(cell);
            Console.WriteLine();
        }

        private static void PrintGridCell(Cell cell) => Render(cell, (ConsoleColor)(cell.ColorIndex + 1), ConsoleColor.White);

        private static void MarkGridCell(Cell cell) => Render(cell, ConsoleColor.White, ConsoleColor.Black);

        private static void Render(Cell cell, ConsoleColor bg, ConsoleColor fg)
        {
            Console.BackgroundColor = bg;
            Console.ForegroundColor = fg;
            Console.Write(cell.Tag);
        }

        public IEnumerable<Cell> FindLargestConnectedArea()
            => FindConnectedAreas()
            .OrderByDescending(area => area.Length)
            .FirstOrDefault();

        private Cell[][] FindConnectedAreas()
        {
            var indices = new int[Size.X, Size.Y];
            var nextIndex = 1;
            foreach (var cell in _cells.OfType<Cell>())
            {
                var sameColoredNeighbours = FindSameColoredNeighbours(cell);
                if (sameColoredNeighbours.Any())
                {
                    var connectedAreaIndices = sameColoredNeighbours.Select(n => indices[n.Pos.X, n.Pos.Y]).ToArray();
                    var minConnectedAreaIndex = connectedAreaIndices.Min();
                    var maxConnectedAreaIndex = connectedAreaIndices.Max();
                    if (maxConnectedAreaIndex > minConnectedAreaIndex)
                        indices.Replace(maxConnectedAreaIndex, minConnectedAreaIndex);
                    indices[cell.Pos.X, cell.Pos.Y] = minConnectedAreaIndex;
                }
                else
                {
                    indices[cell.Pos.X, cell.Pos.Y] = nextIndex++;
                }
            }
            var areas = new Dictionary<int, List<Cell>>();
            for (int x = 0; x < indices.GetLength(0); x++)
                for (int y = 0; y < indices.GetLength(1); y++)
                {
                    var areaIndex = indices[x, y];
                    if (!areas.TryGetValue(areaIndex, out var area))
                        areas[areaIndex] = area = new List<Cell>();
                    area.Add(_cells[x, y]);
                }
            return areas
                .OrderByDescending(p => p.Value.Count)
                .Select((p, i) => p.Value.Select(c => c.Clone((char)(65 + i))).ToArray())
                .ToArray();
        }

        private Cell[] FindSameColoredNeighbours(Cell cell)
            => cell.NorthWestNeighbours(this).Where(n => n.ColorIndex == cell.ColorIndex).ToArray();
    }
}