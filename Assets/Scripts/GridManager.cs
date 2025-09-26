using System;
using Project;
using UnityEngine;

namespace Project.Grid
{
    public struct Cell
    {
        public int x { get; private set; }
        public int y { get; private set; }
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
        }

        public override string ToString() => $"{x}, {y}";
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public bool Equals(Cell other) => x == other.x && y == other.y;
        public override bool Equals(object obj) => obj is Cell other && Equals(other);

        public static bool operator ==(Cell left, Cell right) => left.x == right.x && left.y == right.y;
        public static bool operator !=(Cell left, Cell right) => !(left == right);
    }

    public class GridManager : Singleton<GridManager>
    {
        [SerializeField] private Vector2 cellSize = new Vector2(1, 1);

        public Cell WorldPositionToCell(Vector2 worldPosition)
        {
            int x = worldPosition.x >= 0 ? (int)worldPosition.x + 1 : (int)worldPosition.x - 1;
            int y = worldPosition.y >= 0 ? (int)worldPosition.y + 1 : (int)worldPosition.y - 1;
            // Vector2 cellPos = new Vector2(Mathf.Floor(worldPosition.x + 1f), Mathf.Floor(worldPosition.y + 1f));
            return new Cell(x, y, cellSize);
        }

        public Cell GetNeighborCell(Cell cell, Vector2 direction)
        {
            Vector2 worldPosition = cell.Center + direction;
            return WorldPositionToCell(worldPosition);
        }
    }
}
