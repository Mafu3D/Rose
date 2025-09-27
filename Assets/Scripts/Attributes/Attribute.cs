using System;
using System.Collections.Generic;

namespace Project.Attributes
{
    public enum AttributeType
    {
        Health,
        Armor,
        Strength,
        Magic,
        Dexterity,
        Speed
    }

    public class Attribute
    {
        public readonly AttributeType Type;
        public readonly int StartingValue;
        public readonly int MaxValue;
        public int BaseValue;

        private List<int> statModifiers = new();

        public Attribute(AttributeType type, int startingValue, int maxValue = 99)
        {
            this.Type = type;
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

        public void IncreaseValue(int amount)
        {
            BaseValue = Math.Clamp(BaseValue + amount, 0, MaxValue);
        }

        public void DecreaseValue(int amount)
        {
            BaseValue = Math.Clamp(BaseValue - amount, 0, MaxValue);
        }

        public void RegisterAttributeModifier(int modifier)
        {
            statModifiers.Add(modifier);
        }

        public void DeregisterAttributeModifier(int modifier)
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