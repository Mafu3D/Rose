using System;
using System.Collections.Generic;
using System.Linq;
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
        public void DeregisterFromPath(Cell cell)
        {
            if (registeredPathCells.ContainsKey(cell.GetHashCode()))
            {
                registeredPathCells.Remove(cell.GetHashCode());
                path.Remove(cell);
            }
        }
        public bool TryGetCellInPath(Cell cell) => registeredPathCells.TryGetValue(cell.GetHashCode(), out _);
        public void RegisterToInvalidCells(Cell cell) { if (!invalidCells.ContainsKey(cell.GetHashCode())) invalidCells.Add(cell.GetHashCode(), cell); }
        public bool TryGetCellInInvalidCells(Cell cell) => invalidCells.TryGetValue(cell.GetHashCode(), out _);

        public List<Cell> CalculatePath(bool debug = false)
        {
            // Initialize
            RegisterToInvalidCells(start);
            // List<Cell> potentialStartingPoints;
            // EvaluateNeighbors(start, out potentialStartingPoints, out _);


            Cell current = start;
            int iter = 0;
            while (current != end)
            {
                List<Cell> validNeighbors;
                List<Cell> invalidNeighbors;
                EvaluateNeighbors(current, out validNeighbors, out invalidNeighbors);

                if (validNeighbors.Count == 0)
                {
                    Cell lastCell = path.Last();
                    DeregisterFromPath(lastCell);
                    RegisterToInvalidCells(lastCell);
                    current = path.Last();
                    // Debug.Log($"ran out of valid neighbors at cell: {current}!");
                    // break;
                }
                else
                {
                    Cell next = validNeighbors[0];
                    RegisterToPath(next);
                    current = next;
                }

                iter++;
                if (iter > 1000)
                {
                    Debug.Log("too many iterations!");
                    break;
                }
            }

            RegisterToPath(end);

            if (debug)
            {
                Debug.Log("Calculating path...");
                for (int i = 0; i < path.Count - 1; i++)
                {
                    // This won't work because it is only drawn for one frame!!
                    Debug.DrawLine(path[i].Center, path[i + 1].Center, Color.red);
                }
            }

            return path;
        }

        private void EvaluateNeighbors(Cell cell, out List<Cell> validNeighbors, out List<Cell> invalidNeighbors)
        {
            // Debug.Log($"Evaluating {cell}");

            validNeighbors = new();
            invalidNeighbors = new();

            Cell[] neighbors = grid.GetAllNeighbors(cell);

            int lowestF = 99999999; // Some really big number
            foreach (Cell neighbor in neighbors)
            {
                // Do not check cells that have already been marked as invalidated, are already in the path, or are not walkable
                if (TryGetCellInInvalidCells(neighbor) || TryGetCellInPath(neighbor) || !grid.TryGetCellInWalkableCells(neighbor)) continue;

                int g = DistanceBetween(neighbor, start);
                int h = DistanceBetween(end, neighbor);
                int f = g + h;
                // Debug.Log($"Checking {neighbor} - g: {g}, h: {h}, f: {f}");
                if (f < lowestF)
                {
                    lowestF = f;
                    validNeighbors.Clear();
                    validNeighbors.Add(neighbor);
                }
                else if (f == lowestF)
                {
                    validNeighbors.Add(neighbor);
                }
                else
                {
                    invalidNeighbors.Add(neighbor);
                }
            }
            foreach (Cell invalidNeighbor in invalidNeighbors)
            {
                RegisterToInvalidCells(invalidNeighbor);
            }
            // string result = "Potential Cells: ";
            // if (validNeighbors.Count > 0)
            // {
            //     foreach (Cell c in validNeighbors)
            //     {
            //         result += $"{c} ";
            //     }
            // }
            // else
            // {
            //     result += "None";
            // }
            // Debug.Log(result);
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