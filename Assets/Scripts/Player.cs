using Project.Hero;
using UnityEngine;

namespace Project.PlayerSystem
{
    public class Player : MonoBehaviour
    {
        [SerializeField] public Input.InputReader InputReader;
        [SerializeField] public HeroData HeroData;
        public Camera PlayerCamera { get; private set; }

        void Awake()
        {
            PlayerCamera = Camera.main;
        }
    }
}