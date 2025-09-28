using Project.GameNode.Hero;
using Project.Items;
using UnityEngine;

namespace Project.PlayerSystem
{
    public class Player : MonoBehaviour
    {
        [SerializeField] public Input.InputReader InputReader;
        [SerializeField] public HeroNode HeroNode;
        [SerializeField] public Inventory Inventory;
        public Camera PlayerCamera { get; private set; }

        void Awake()
        {
            PlayerCamera = Camera.main;
        }
    }
}