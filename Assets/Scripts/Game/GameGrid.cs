using System;
using System.Collections.Generic;
using System.Linq;
using Project;
using UnityEngine;
using Project.GameTiles;
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

        private Dictionary<Cell, List<Tile>> registeredCells = new();
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

        public bool AreTilesRegisteredToCell(Cell cell, bool excludeHeroes = true)
        {
            List<Tile> registeredTiles = GetTilesRegisteredToCell(cell);
            if (registeredTiles.Count > 0)
            {
                if (registeredTiles.Count == 1 && excludeHeroes)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public bool TryGetTileesRegisteredToCell(Cell cell, out List<Tile> registeredTiles, bool excludeHeroes = true)
        {
            registeredTiles = GetTilesRegisteredToCell(cell, excludeHeroes);
            if (registeredTiles.Count > 0) return true;
            return false;
        }

        private List<Tile> GetTilesRegisteredToCell(Cell cell, bool excludeHeroes = true)
        {
            List<Tile> registeredTiles = new List<Tile>();
            Cell registeredCell;
            registeredKeys.TryGetValue(cell.GetHashCode(), out registeredCell);
            if (registeredCell != null)
            {
                List<Tile> tiles;
                registeredCells.TryGetValue(registeredCell, out tiles);
                if (tiles != null)
                {
                    // if (excludeHeroes) tiles.RemoveAll(n => GameManager.Instance.Player.HeroTile);
                    // registeredTiles = tiles;
                    foreach (Tile tile in tiles)
                    {
                        if (tile.IsPlayer != true && excludeHeroes) registeredTiles.Add(tile);
                    }
                }
            }
            return registeredTiles.OrderByDescending(x => x.TileData.ActivationPriority).ToList();
        }

        public List<Tile> GetAllRegisteredTiles()
        {
            List<Tile> registeredTiles = new();
            foreach (KeyValuePair<Cell, List<Tile>> cells in registeredCells)
            {
                foreach (Tile tile in cells.Value)
                {
                    registeredTiles.Add(tile);
                }
            }
            return registeredTiles;
        }

        public void RegisterToCell(Cell cell, Tile tile)
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
                registeredCells.Add(registeredCell, new List<Tile>());
            }

            if (!registeredCells[registeredCell].Contains(tile))
            {
                registeredCells[registeredCell].Add(tile);
            }

        }

        public void DeregisterFromCell(Cell cell, Tile tile)
        {
            int hash = cell.GetHashCode();
            if (registeredKeys.ContainsKey(hash))
            {
                Cell registeredCell = registeredKeys[hash];

                if (registeredCells.ContainsKey(registeredCell))
                {
                    if (registeredCells[registeredCell].Contains(tile))
                    {
                        registeredCells[registeredCell].Remove(tile);
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

        public List<Cell> GetPathBetweenTwoCells(Cell start, Cell end, bool debug = false)
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
