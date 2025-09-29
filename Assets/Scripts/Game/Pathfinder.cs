using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class Pathfinder
    {
        GameGrid grid;
        Cell start;
        Cell end;

        List<Cell> path = new();
        private Dictionary<int, Cell> registeredPathCells = new();
        private Dictionary<int, Cell> invalidCells = new();

        public Pathfinder(GameGrid grid, Cell start, Cell end)
        {
            this.grid = grid;
            this.start = start;
            this.end = end;
        }

        public void RegisterToPath(Cell cell)
        {
            if (!registeredPathCells.ContainsKey(cell.GetHashCode()))
            {
                registeredPathCells.Add(cell.GetHashCode(), cell);
                path.Add(cell);
            }
        }
        public bool TryGetCellInPath(Cell cell) => registeredPathCells.TryGetValue(cell.GetHashCode(), out _);
        public void RegisterToInvalidCells(Cell cell) { if (!invalidCells.ContainsKey(cell.GetHashCode())) invalidCells.Add(cell.GetHashCode(), cell); }
        public bool TryGetCellInInvalidCells(Cell cell) => invalidCells.TryGetValue(cell.GetHashCode(), out _);

        public List<Cell> CalculatePath()
        {
            // only invalidate nodes that were actually checked

            Cell current = start;
            int i = 0;
            while (current != end)
            {
                Cell[] neighbors = grid.GetAllNeighbors(current);

                Cell next;
                Cell bestNeighbor = neighbors[0];
                List<Cell> invalidNeighbors = new();
                int lowestF = 99999999;
                Debug.Log(current);
                foreach (Cell neighbor in neighbors)
                {
                    // Do not check cells that have already been marked as invalidated, are already in the path, or are not walkable
                    if (TryGetCellInInvalidCells(neighbor) || TryGetCellInPath(neighbor) || !grid.TryGetCellInWalkableCells(neighbor)) continue;

                    int g = DistanceBetween(neighbor, start);
                    int h = DistanceBetween(end, neighbor);
                    int f = g + h;
                    if (f < lowestF)
                    {
                        lowestF = f;
                        if (bestNeighbor != neighbor)
                        {
                            invalidNeighbors.Add(bestNeighbor);
                            bestNeighbor = neighbor;
                        }
                    }
                    else
                    {
                        invalidNeighbors.Add(bestNeighbor);
                    }
                }
                foreach (Cell invalidNeighbor in invalidNeighbors)
                {
                    RegisterToInvalidCells(invalidNeighbor);
                }
                next = bestNeighbor;
                Debug.Log(invalidCells.Count);

                RegisterToPath(next);
                current = next;

                i++;
                if (i > 1000)
                {
                    Debug.Log("too many iterations!");
                    break;
                }
            }
            return path;
        }

        private int DistanceBetween(Cell A, Cell B)
        {
            Vector2 difference = A.Center - B.Center;
            difference.x = Math.Abs(difference.x);
            difference.y = Math.Abs(difference.y);
            return (int)difference.x + (int)difference.y;
        }
    }
}