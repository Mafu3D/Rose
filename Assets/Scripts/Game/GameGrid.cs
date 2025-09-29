using System;
using System.Collections.Generic;
using System.Linq;
using Project;
using UnityEngine;
using Project.GameNode;
using Project.GameNode.Hero;
using System.IO;

namespace Project
{
    public struct Cell
    {
        public int x { get; private set; }
        public int y { get; private set; }
        readonly int hashCode;
        Vector2 cellSize;

        public Vector2 Center
        {
            get
            {
                float x = this.x >= 0 ? this.x - 1 + cellSize.x / 2f : this.x + 1 - cellSize.x / 2f;
                float y = this.y >= 0 ? this.y - 1 + cellSize.y / 2f : this.y + 1 - cellSize.y / 2f;

                return new Vector2(x, y);
            }
        }

        public Cell(int x, int y, Vector2 cellSize)
        {
            this.x = x;
            this.y = y;
            this.cellSize = cellSize;
            this.hashCode = $"{x}{y}".ToFNV1aHash();
        }

        public override string ToString() => $"{x}, {y}";
        public override int GetHashCode() => hashCode;
        public bool Equals(Cell other) => x == other.x && y == other.y;
        public override bool Equals(object obj) => obj is Cell other && Equals(other);

        public static bool operator ==(Cell left, Cell right) => left.hashCode == right.hashCode;
        public static bool operator !=(Cell left, Cell right) => !(left == right);
    }

    public class GameGrid
    {
        private Vector2 cellSize = new Vector2(1, 1);

        private Dictionary<Cell, List<Node>> registeredCells = new();
        private Dictionary<int, Cell> registeredKeys = new();

        private Dictionary<int, Cell> walkableCells = new();

        public GameGrid(Vector2 cellSize)
        {
            this.cellSize = cellSize;
        }

        public void RegisterWalkableCells(List<Cell> cells)
        {
            foreach (Cell cell in cells)
            {
                int hash = cell.GetHashCode();
                if (!walkableCells.ContainsKey(hash))
                {
                    walkableCells.Add(hash, cell);
                }
            }
        }

        public bool TryGetCellInWalkableCells(Cell cell)
        {
            int hash = cell.GetHashCode();
            return walkableCells.TryGetValue(hash, out _);
        }

        public bool AreNodesRegisteredToCell(Cell cell, bool excludeHeroes = true)
        {
            List<Node> registeredNodes = GetNodesRegisteredToCell(cell);
            if (registeredNodes.Count > 0)
            {
                if (registeredNodes.Count == 1 && excludeHeroes)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public bool TryGetNodesRegisteredToCell(Cell cell, out List<Node> registeredNodes, bool excludeHeroes = true)
        {
            registeredNodes = GetNodesRegisteredToCell(cell, excludeHeroes);
            if (registeredNodes.Count > 0) return true;
            return false;
        }

        private List<Node> GetNodesRegisteredToCell(Cell cell, bool excludeHeroes = true)
        {
            List<Node> registeredNodes = new List<Node>();
            Cell registeredCell;
            registeredKeys.TryGetValue(cell.GetHashCode(), out registeredCell);
            if (registeredCell != null)
            {
                List<Node> nodes;
                registeredCells.TryGetValue(registeredCell, out nodes);
                if (nodes != null)
                {
                    if (excludeHeroes) nodes.RemoveAll(n => n is HeroNode);
                    registeredNodes = nodes;
                }
            }
            return registeredNodes.OrderByDescending(x => x.NodeData.Priority).ToList();
        }

        public void RegisterToCell(Cell cell, Node node)
        {
            Cell registeredCell;
            int hash = cell.GetHashCode();
            if (!registeredKeys.ContainsKey(hash))
            {
                registeredKeys.Add(hash, cell);
                registeredCell = cell;
            }
            else
            {
                registeredCell = registeredKeys[hash];
            }

            if (!registeredCells.ContainsKey(registeredCell))
            {
                registeredCells.Add(registeredCell, new List<Node>());
            }

            if (!registeredCells[registeredCell].Contains(node))
            {
                registeredCells[registeredCell].Add(node);
            }

        }

        public void DeregisterFromCell(Cell cell, Node node)
        {
            int hash = cell.GetHashCode();
            if (registeredKeys.ContainsKey(hash))
            {
                Cell registeredCell = registeredKeys[hash];

                if (registeredCells.ContainsKey(registeredCell))
                {
                    if (registeredCells[registeredCell].Contains(node))
                    {
                        registeredCells[registeredCell].Remove(node);
                    }
                    if (registeredCells[registeredCell].Count == 0)
                    {
                        registeredCells.Remove(registeredCell);
                    }
                }

                registeredKeys.Remove(hash);
            }
        }

        public void DebugRegisteredCells()
        {
            string outputString = "";
            foreach (var key in registeredCells.Keys)
            {
                outputString += $"{key.ToString()}: \n";
                foreach (var value in registeredCells[key])
                {
                    outputString += $"     {value} \n";
                }
            }
            Debug.Log(outputString);
        }

        public Cell WorldPositionToCell(Vector2 worldPosition)
        {
            int x = worldPosition.x >= 0 ? (int)worldPosition.x + 1 : (int)worldPosition.x - 1;
            int y = worldPosition.y >= 0 ? (int)worldPosition.y + 1 : (int)worldPosition.y - 1;
            return new Cell(x, y, cellSize);
        }

        public Cell GetNeighborCell(Cell cell, Vector2 direction)
        {
            Vector2 worldPosition = cell.Center + direction;
            return WorldPositionToCell(worldPosition);
        }

        public List<Cell> GetPathBetweenTwoCells(Cell start, Cell end)
        {
            Pathfinder pathfinder = new Pathfinder(this, start, end);
            return pathfinder.CalculatePath();
        }

        public Cell[] GetAllNeighbors(Cell cell)
        {
            Cell[] neighbors = new Cell[4];
            neighbors[0] = GetNeighborCell(cell, new Vector2(0, 1));
            neighbors[1] = GetNeighborCell(cell, new Vector2(1, 0));
            neighbors[2] = GetNeighborCell(cell, new Vector2(0, -1));
            neighbors[3] = GetNeighborCell(cell, new Vector2(-1, 0));
            return neighbors;
        }

        private int CellsBetween(Cell A, Cell B)
        {
            Vector2 difference = A.Center - B.Center;
            difference.x = Math.Abs(difference.x);
            difference.y = Math.Abs(difference.y);
            return (int)difference.x + (int)difference.y;
        }
    }
}
