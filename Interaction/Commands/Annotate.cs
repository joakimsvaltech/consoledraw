﻿using ConsoleDraw.Core.Geometry;
using ConsoleDraw.Core.Interaction;
using ConsoleDraw.Interaction.Operations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.Core
{
    public class Annotate : Command
    {
        public Annotate(Canvas grid) : base(grid, "_Annotate") {}

        public override IExecutable CreateOperation() => new Operation(Grid);

        private class Operation : Undoable
        {
            private Cell[] _oldCells = new Cell[0];
            private Cell[] _newCells = new Cell[0];

            public Operation(Canvas grid) : base(grid) { }

            protected override bool DoExecute()
            {
                _oldCells = Canvas.Cells.Where(c => c.Brush.Shape != ' ').ToArray();
                Unnotate(_oldCells);
                _newCells = GetAnnotations();
                Canvas.Annotate(_newCells);
                return !_oldCells.SequenceEqual(_newCells);
            }

            protected override bool DoUndo() => Reannotate(_newCells, _oldCells);

            protected override bool DoRedo() => Reannotate(_oldCells, _newCells);

            private bool Reannotate(Cell[] from, Cell[] to)
            {
                if (from.SequenceEqual(to))
                    return false;
                Unnotate(from);
                Canvas.Annotate(to);
                return true;
            }

            private void Unnotate(IEnumerable<Cell> cells)
            {
                Canvas.Annotate(cells.Select(c => c.Clone(' ')).ToArray());
            }

            private Cell[] GetAnnotations()
                => FindConnectedAreas()
                .OrderByDescending(area => area.Length)
                .First().ToArray();

            private Cell[][] FindConnectedAreas()
            {
                var indices = new int[Canvas.Size.X + 1, Canvas.Size.Y + 1];
                var nextIndex = 1;
                foreach (var cell in Canvas.Cells)
                {
                    var sameColoredNeighbours = FindSameColoredNeighbours(cell);
                    if (sameColoredNeighbours.Any())
                    {
                        var connectedAreaIndices = sameColoredNeighbours.Select(n => indices[n.Pos.X, n.Pos.Y]).ToArray();
                        var minConnectedAreaIndex = connectedAreaIndices.Min();
                        var maxConnectedAreaIndex = connectedAreaIndices.Max();
                        if (maxConnectedAreaIndex > minConnectedAreaIndex)
                            Replace(indices, maxConnectedAreaIndex, minConnectedAreaIndex);
                        indices[cell.Pos.X, cell.Pos.Y] = minConnectedAreaIndex;
                    }
                    else
                    {
                        indices[cell.Pos.X, cell.Pos.Y] = nextIndex++;
                    }
                }
                var areas = new Dictionary<int, List<Cell>>();
                foreach (Point pos in Canvas.Positions)
                {
                    var areaIndex = indices[pos.X, pos.Y];
                    if (!areas.TryGetValue(areaIndex, out var area))
                        areas[areaIndex] = area = new List<Cell>();
                    area.Add(Canvas[pos]);
                }
                return areas
                    .OrderByDescending(p => p.Value.Count)
                    .Select((p, i) => p.Value.Select(c => c.Clone((char)(65 + i))).ToArray())
                    .ToArray();
            }

            private Cell[] FindSameColoredNeighbours(Cell cell)
                => cell.NorthWestNeighbours(Canvas)
                .Where(n => n.Brush.Background == cell.Brush.Background).ToArray();

            private void Replace(int[,] grid, int replace, int with)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                    for (int y = 0; y < grid.GetLength(1); y++)
                        if (grid[x, y] == replace)
                            grid[x, y] = with;
            }
        }
    }
}