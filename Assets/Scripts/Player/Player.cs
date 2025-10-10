using Project.GameTiles;
using Project.Items;
using UnityEngine;

namespace Project.PlayerSystem
{
    public class Player : MonoBehaviour
    {
        [SerializeField] public Input.InputReader InputReader;
        [SerializeField] public Tile HeroTile;
        [SerializeField] public Inventory Inventory;
        [SerializeField] public GoldTracker GoldTracker;
        [SerializeField] public GemTracker GemTracker;
        public Camera PlayerCamera { get; private set; }

        void Awake()
        {
            PlayerCamera = Camera.main;
        }
    }
}