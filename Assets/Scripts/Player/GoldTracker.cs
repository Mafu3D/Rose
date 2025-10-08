using System;
using UnityEngine;

namespace Project
{
    public class GoldTracker : MonoBehaviour
    {
        public int Gold { get; private set; }

        [field: SerializeField] public int StartingGold { get; private set; } = 0;
        [field: SerializeField] public int MaxGold { get; private set; } = 99;

        public event Action OnGoldChangedEvent;

        void Start()
        {
            Gold = StartingGold;
        }

        public void AddGold(int amount)
        {
            Gold = Math.Clamp(Gold + Math.Abs(amount), 0, MaxGold);
            OnGoldChangedEvent?.Invoke();
        }

        public void RemoveGold(int amount)
        {
            Gold = Math.Clamp(Gold - Math.Abs(amount), 0, MaxGold);
            OnGoldChangedEvent?.Invoke();
        }

    }
}