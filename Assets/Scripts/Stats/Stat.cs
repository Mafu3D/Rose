using System.Collections.Generic;

namespace Project.Stats
{
    public class Stat
    {
        public readonly string Name;
        public readonly int StartingValue;

        private List<int> statModifiers = new();

        public Stat(string name, int startingValue)
        {
            this.Name = name;
            this.StartingValue = startingValue;
        }

        public int GetValue()
        {
            int value = StartingValue;
            foreach (int modifier in statModifiers)
            {
                value += modifier;
            }
            return value;
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