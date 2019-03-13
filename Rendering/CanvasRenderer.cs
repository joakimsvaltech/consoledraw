using ConsoleDraw.Core;
using ConsoleDraw.Core.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.Rendering
{
    public class CanvasRenderer
    {
        private readonly Canvas _canvas;
        private readonly Point _origin;
        private Point[] _currentMarkedArea = new Point[0];
        private Point _currentMarkedPos;

        public CanvasRenderer(Canvas canvas, Point origin)
        {
            _canvas = canvas;
            _origin = origin;
            _canvas.CurrentPositionChanged += Canvas_CurrentPositionChanged;
            _canvas.Highlight.AreaChanged += Highlight_AreaChanged;
            _canvas.CellChanged += Canvas_CellChanged;
            _canvas.CellsChanged += Canvas_CellsChanged;
        }

        public void Render()
        {
            _canvas.Positions.ForEach(Render);
            Mark(_currentMarkedPos);
        }

        public void Fill(IEnumerable<Cell> area)
        {
            area.ForEach(Render);
        }

        public void Unmark(params Point[] points)
        {
            points.ForEach(Render);
        }

        public void Mark(params Point[] points)
        {
            points.ForEach(Mark);
        }

        private void Canvas_CellsChanged(object sender, EventArgs<Cell[]> e)
        {
            e.Model.ForEach(Render);
            Mark(_currentMarkedPos);
        }

        private void Canvas_CellChanged(object sender, EventArgs<Cell> e)
        {
            Render(e);
            Mark(_currentMarkedPos);
        }

        private void Highlight_AreaChanged(object sender, EventArgs<Point[]> e)
        {
            if (e.Model.Any())
            {
                var remove = _currentMarkedArea
                    .Append(_currentMarkedPos)
                    .Except(e.Model)
                    .ToArray();
                var add = e.Model
                    .Except(_currentMarkedArea)
                    .ToArray();
                Unmark(remove);
                Mark(add);
            }
            else
            {
                Unmark(_currentMarkedArea);
                Mark(_currentMarkedPos = _canvas.CurrentPos);
            }
            _currentMarkedArea = e;
        }

        private void Canvas_CurrentPositionChanged(object sender, EventArgs<Point> e)
        {
            var oldPos = _currentMarkedPos;
            _currentMarkedPos = e;
            if (_currentMarkedArea.Any() || oldPos == _currentMarkedPos)
                return;
            Unmark(oldPos);
            Mark(_currentMarkedPos);
        }

        private void Render(Point pos) => Render(_canvas[pos]);

        private void Render(Cell cell)
        {
            PositionCursor(cell.Pos);
            Render(cell, cell.Brush);
        }

        private void Mark(Point pos) => Mark(_canvas[pos]);

        private void Mark(Cell cell) => Render(cell, (ConsoleColor.White, ConsoleColor.Black));

        private void Render(Cell cell, Brush brush)
        {
            Console.BackgroundColor = brush.Background;
            Console.ForegroundColor =brush.Foreground;
            PositionCursor(cell.Pos);
            Console.Write(brush.Shape);
        }

        private void PositionCursor(Point pos)
        {
            Renderer.CursorPosition = _origin + pos;
        }
    }
}