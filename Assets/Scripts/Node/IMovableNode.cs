using UnityEngine;

namespace Project.GameTiles {
    public interface IMovableTile {
        public void Move(Vector2 direction);
    }
}