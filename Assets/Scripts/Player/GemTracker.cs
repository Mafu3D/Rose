using System;
using UnityEngine;

namespace Project
{
    public class GemTracker : MonoBehaviour
    {
        public int Gem { get; private set; }

        [field: SerializeField] public int StartingGem { get; private set; } = 0;
        [field: SerializeField] public int MaxGem { get; private set; } = 99;

        public event Action OnGemChangedEvent;

        void Start()
        {
            Gem = StartingGem;
        }

        public void AddGem(int amount)
        {
            Gem = Math.Clamp(Gem + Math.Abs(amount), 0, MaxGem);
            OnGemChangedEvent?.Invoke();
        }

        public void RemoveGem(int amount)
        {
            Gem = Math.Clamp(Gem - Math.Abs(amount), 0, MaxGem);
            OnGemChangedEvent?.Invoke();
        }

    }
}