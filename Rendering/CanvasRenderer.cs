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

        private void Canvas_CellsChanged(object sender, Core.Events.CellsEventArgs e)
        {
            e.Cells.ForEach(Render);
            Mark(_currentMarkedPos);
        }

        private void Canvas_CellChanged(object sender, Core.Events.CellEventArgs e)
        {
            Render(e.Cell);
            Mark(_currentMarkedPos);
        }

        private void Highlight_AreaChanged(object sender, Core.Events.PointsEventArgs e)
        {
            if (e.Points.Any())
            {
                var remove = _currentMarkedArea
                    .Append(_currentMarkedPos)
                    .Except(e.Points)
                    .ToArray();
                var add = e.Points
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
            _currentMarkedArea = e.Points;
        }

        private void Canvas_CurrentPositionChanged(object sender, Core.Events.PointEventArgs e)
        {
            var oldPos = _currentMarkedPos;
            _currentMarkedPos = e.Point;
            if (_currentMarkedArea.Any() || oldPos == _currentMarkedPos)
                return;
            Unmark(oldPos);
            Mark(_currentMarkedPos);
        }

        public void Render()
        {
            _canvas.Positions.ForEach(Render);
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

        private void Render(Point pos) => Render(_canvas[pos]);

        private void Render(Cell cell)
        {
            PositionCursor(cell.Pos);
            Render(cell, cell.Color, ConsoleColor.White);
        }

        private void Mark(Point pos) => Mark(_canvas[pos]);

        private void Mark(Cell cell) => Render(cell, ConsoleColor.White, ConsoleColor.Black);

        private void Render(Cell cell, ConsoleColor bg, ConsoleColor fg)
        {
            Console.BackgroundColor = bg;
            Console.ForegroundColor = fg;
            PositionCursor(cell.Pos);
            Console.Write(cell.Tag);
        }

        private void PositionCursor(Point pos)
        {
            Renderer.PositionCursor(_origin + pos);
        }
    }
}