using System;
using System.Collections.Generic;

namespace Project.Stats
{
    public class Stat
    {
        public readonly string Name;
        public readonly int StartingValue;
        public readonly int MaxValue;
        public int BaseValue;

        private List<int> statModifiers = new();

        public Stat(string name, int startingValue, int maxValue = 99)
        {
            this.Name = name;
            this.StartingValue = startingValue;
            this.BaseValue = startingValue;
            this.MaxValue = maxValue;
        }

        public int GetValue()
        {
            int value = BaseValue;
            foreach (int modifier in statModifiers)
            {
                value += modifier;
            }
            return value;
        }

        public void IncreaseStat(int amount)
        {
            BaseValue = Math.Clamp(BaseValue + amount, 0, MaxValue);
        }

        public void DecreaseStat(int amount)
        {
            BaseValue = Math.Clamp(BaseValue - amount, 0, MaxValue);
        }

        public void RegisterStatModifier(int modifier)
        {
            statModifiers.Add(modifier);
        }

        public void DeregisterStatModifier(int modifier)
        {
            if (statModifiers.Contains(modifier))
            {
                statModifiers.Remove(modifier);
            }
        }
    }

    // public struct StatModifier {
    //     public readonly int Value;

    //     public StatModifier(int value)
    //     {
    //         this.Value = value;
    //     }
    // }
}