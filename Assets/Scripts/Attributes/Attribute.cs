using System;
using System.Collections.Generic;
using UnityEngine;

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
        public int MaxValue;
        public int BaseValue;

        private List<int> baseValueModifiers = new();
        private List<int> maxValueModifiers = new();

        public event Action OnValueChanged;

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
            foreach (int modifier in baseValueModifiers)
            {
                value = Math.Clamp(value + modifier, -99, MaxValue);
            }
            return value;
        }

        public int GetMaxValue()
        {
            int value = MaxValue;
            foreach (int modifier in maxValueModifiers)
            {
                value += modifier;
            }
            return value;
        }

        public void ModifyValue(int amount)
        {
            BaseValue = Math.Clamp(BaseValue + amount, 0, MaxValue);
            OnValueChanged?.Invoke();
        }

        public void ModifyMaxValue(int amount)
        {
            MaxValue = Math.Clamp(MaxValue + amount, 0, 99);
            OnValueChanged?.Invoke();
        }

        public void RegisterBaseAttributeModifier(int modifier)
        {
            baseValueModifiers.Add(modifier);
            OnValueChanged?.Invoke();
        }

        public void DeregisterBaseAttributeModifier(int modifier)
        {
            if (baseValueModifiers.Contains(modifier))
            {
                baseValueModifiers.Remove(modifier);
            }
            OnValueChanged?.Invoke();
        }

        public void RegisterMaxAttributeModifier(int modifier)
        {
            maxValueModifiers.Add(modifier);
            OnValueChanged?.Invoke();
        }

        public void DeregisterMaxAttributeModifier(int modifier)
        {
            if (maxValueModifiers.Contains(modifier))
            {
                maxValueModifiers.Remove(modifier);
            }
            OnValueChanged?.Invoke();
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