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
        Speed,
        Multistrike
    }

    public class AttributeModifier
    {
        public int Value { get; private set; }
        public int Duration;
        public AttributeModifier(int value, int duration)
        {
            this.Value = value;
            this.Duration = duration;
        }
    }

    public class Attribute
    {
        public readonly AttributeType Type;
        public readonly int StartingValue;
        public int MaxValue;
        public int BaseValue;

        private List<AttributeModifier> baseValueModifiers = new();
        private List<AttributeModifier> maxValueModifiers = new();

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
            foreach (AttributeModifier mod in baseValueModifiers)
            {
                copy.RegisterBaseAttributeModifier(mod.Value, mod.Duration);
            }
            foreach (AttributeModifier mod in maxValueModifiers)
            {
                copy.RegisterBaseAttributeModifier(mod.Value, mod.Duration);
            }
            return copy;
        }

        public int GetValue()
        {
            // int preMod = BaseValue;
            int value = BaseValue;
            foreach (AttributeModifier modifier in baseValueModifiers)
            {
                value = Math.Clamp(value + modifier.Value, 0, GetMaxValue());
            }
            // Debug.Log($"Get Base {Type}: {preMod} >>> {value}");
            return value;
        }

        public int GetMaxValue()
        {
            // int preMod = MaxValue;
            int value = MaxValue;
            foreach (AttributeModifier modifier in maxValueModifiers)
            {
                value += modifier.Value;
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

        public void RegisterBaseAttributeModifier(int modifier, int duration=-1)
        {
            AttributeModifier attributeModifier = new AttributeModifier(modifier, duration);
            // Debug.Log($"Register to Base {Type}: {modifier}");
            baseValueModifiers.Add(attributeModifier);
            OnValueChanged?.Invoke();
        }

        public void DeregisterBaseAttributeModifier(int modifier)
        {
            AttributeModifier modifierToRemove = null;
            foreach (AttributeModifier attributeModifier in baseValueModifiers)
            {
                if (attributeModifier.Value == modifier)
                {
                    modifierToRemove = attributeModifier;
                    break;

                }
            }
            if (modifierToRemove != null)
            {
                baseValueModifiers.Remove(modifierToRemove);
                OnValueChanged?.Invoke();
            }
        }

        public void DeregisterBaseAttributeModifier(AttributeModifier modifier)
        {
            if (baseValueModifiers.Contains(modifier))
            {
                baseValueModifiers.Remove(modifier);
                OnValueChanged?.Invoke();
            }
        }

        public void RegisterMaxAttributeModifier(int modifier, int duration=-1)
        {
            AttributeModifier attributeModifier = new AttributeModifier(modifier, duration);
            // Debug.Log($"Register to Max {Type}: {modifier}");
            maxValueModifiers.Add(attributeModifier);
            OnValueChanged?.Invoke();
        }

        public void DeregisterMaxAttributeModifier(int modifier)
        {
            AttributeModifier modifierToRemove = null;
            foreach (AttributeModifier attributeModifier in maxValueModifiers)
            {
                if (attributeModifier.Value == modifier)
                {
                    modifierToRemove = attributeModifier;
                    break;
                }
            }
            if (modifierToRemove != null)
            {
                maxValueModifiers.Remove(modifierToRemove);
                OnValueChanged?.Invoke();
            }
            OnValueChanged?.Invoke();
        }

        public void DeregisterMaxAttributeModifier(AttributeModifier modifier)
        {
            if (maxValueModifiers.Contains(modifier))
            {
                maxValueModifiers.Remove(modifier);
                OnValueChanged?.Invoke();
            }
        }

        public void TickAttributeModifiers(int amount=1)
        {
            List<AttributeModifier> expiredBaseAttributeModifiers = new();
            foreach (AttributeModifier attributeModifier in baseValueModifiers)
            {
                if (attributeModifier.Duration >= 0)
                {
                    attributeModifier.Duration -= amount;
                    if (attributeModifier.Duration <= 0)
                    {
                        expiredBaseAttributeModifiers.Add(attributeModifier);
                    }
                }
            }
            foreach (AttributeModifier attributeModifier in expiredBaseAttributeModifiers)
            {
                DeregisterBaseAttributeModifier(attributeModifier);
            }

            List<AttributeModifier> expiredMaxAttributeModifiers = new();
            foreach (AttributeModifier attributeModifier in maxValueModifiers)
            {
                if (attributeModifier.Duration >= 0)
                {
                    attributeModifier.Duration -= amount;
                    if (attributeModifier.Duration <= 0)
                    {
                        expiredMaxAttributeModifiers.Add(attributeModifier);
                    }
                }
            }
            foreach(AttributeModifier attributeModifier in expiredMaxAttributeModifiers)
            {
                DeregisterMaxAttributeModifier(attributeModifier);
            }
        }
    }
}