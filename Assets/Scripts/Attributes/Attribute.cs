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

        public Attribute Copy()
        {
            Attribute copy = new Attribute(Type, StartingValue, MaxValue);
            copy.SetBaseValue(BaseValue);
            copy.SetMaxValue(MaxValue);
            foreach (int mod in baseValueModifiers)
            {
                copy.RegisterBaseAttributeModifier(mod);
            }
            foreach (int mod in maxValueModifiers)
            {
                copy.RegisterBaseAttributeModifier(mod);
            }
            return copy;
        }

        public int GetValue()
        {
            // int preMod = BaseValue;
            int value = BaseValue;
            foreach (int modifier in baseValueModifiers)
            {
                value = Math.Clamp(value + modifier, 0, GetMaxValue());
            }
            // Debug.Log($"Get Base {Type}: {preMod} >>> {value}");
            return value;
        }

        public int GetMaxValue()
        {
            // int preMod = MaxValue;
            int value = MaxValue;
            foreach (int modifier in maxValueModifiers)
            {
                value += modifier;
            }
            //  Debug.Log($"Get Max {Type}: {preMod} >>> {value}");
            return value;
        }

        public void SetBaseValue(int value)
        {
            BaseValue = value;
        }

        public void SetMaxValue(int value)
        {
            MaxValue = value;
        }

        public void ModifyValue(int amount)
        {
            // int preBase = BaseValue;
            // int preCalc = BaseValue + amount;
            BaseValue = Math.Clamp(BaseValue + amount, 0, GetMaxValue());
            OnValueChanged?.Invoke();
            // Debug.Log($"Modify Base {Type}: {BaseValue} / {MaxValue} --- {BaseValue} + {amount} = {preCalc}");
        }

        public void ModifyMaxValue(int amount)
        {
            // int preMax = MaxValue;
            // int preCalc = MaxValue + amount;
            MaxValue = Math.Clamp(MaxValue + amount, 0, 99);
            OnValueChanged?.Invoke();
            // Debug.Log($"Modify Max {Type}: {MaxValue} --- {preMax} + {amount} = {preCalc}");
        }

        public void RegisterBaseAttributeModifier(int modifier)
        {
            // Debug.Log($"Register to Base {Type}: {modifier}");
            baseValueModifiers.Add(modifier);
            OnValueChanged?.Invoke();
        }

        public void DeregisterBaseAttributeModifier(int modifier)
        {
            if (baseValueModifiers.Contains(modifier))
            {
                // Debug.Log($"Deregister to Base {Type}: {modifier}");
                baseValueModifiers.Remove(modifier);
            }
            OnValueChanged?.Invoke();
        }

        public void RegisterMaxAttributeModifier(int modifier)
        {
            // Debug.Log($"Register to Max {Type}: {modifier}");
            maxValueModifiers.Add(modifier);
            OnValueChanged?.Invoke();
        }

        public void DeregisterMaxAttributeModifier(int modifier)
        {
            if (maxValueModifiers.Contains(modifier))
            {
                // Debug.Log($"Deregister to Max {Type}: {modifier}");
                maxValueModifiers.Remove(modifier);
            }
            OnValueChanged?.Invoke();
        }
    }
}