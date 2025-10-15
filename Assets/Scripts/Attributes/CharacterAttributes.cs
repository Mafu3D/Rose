using System.Collections.Generic;
using UnityEngine;

namespace Project.Attributes
{
    public class CharacterAttributes
    {
        Dictionary<AttributeType, Attribute> Attributes = new();

        public CharacterAttributes(AttributesData data)
        {
            Attributes.Add(AttributeType.Health, new Attribute(AttributeType.Health, data.Health, data.Health));
            Attributes.Add(AttributeType.Armor, new Attribute(AttributeType.Armor, data.Armor));
            Attributes.Add(AttributeType.Strength, new Attribute(AttributeType.Strength, data.Strength));
            Attributes.Add(AttributeType.Magic, new Attribute(AttributeType.Magic, data.Magic));
            Attributes.Add(AttributeType.Dexterity, new Attribute(AttributeType.Dexterity, data.Dexterity));
            Attributes.Add(AttributeType.Speed, new Attribute(AttributeType.Speed, data.Speed));

            Attributes.Add(AttributeType.Multistrike, new Attribute(AttributeType.Multistrike, data.Multistrike));
        }

        public CharacterAttributes() {

        }

        public CharacterAttributes Copy()
        {
            CharacterAttributes copy = new CharacterAttributes();

            Dictionary<AttributeType, Attribute> copyAttributes = new()
            {
                { AttributeType.Health, GetAttribute(AttributeType.Health).Copy() },
                { AttributeType.Armor, GetAttribute(AttributeType.Armor).Copy() },
                { AttributeType.Strength, GetAttribute(AttributeType.Strength).Copy() },
                { AttributeType.Magic, GetAttribute(AttributeType.Magic).Copy() },
                { AttributeType.Speed, GetAttribute(AttributeType.Speed).Copy() },
                { AttributeType.Dexterity, GetAttribute(AttributeType.Dexterity).Copy() },

                { AttributeType.Multistrike, GetAttribute(AttributeType.Multistrike).Copy() }
            };

            copy.Attributes = copyAttributes;

            return copy;
        }

        public void ReplaceAttribute(AttributeType type, Attribute attribute)
        {
            Attributes.Remove(type);
            Attributes.Add(type, attribute);
        }

        public Attribute GetAttribute(AttributeType type)
        {
            Attribute attribute;
            if (Attributes.TryGetValue(type, out attribute))
            {
                return attribute;
            }
            return default;
        }

        public int GetAttributeValue(AttributeType type)
        {
            Attribute attribute;
            if (Attributes.TryGetValue(type, out attribute))
            {
                return attribute.GetValue();
            }
            return 0;
        }

        public void SetAttributeBaseValue(AttributeType type, int value)
        {
            Attribute attribute;
            if (Attributes.TryGetValue(type, out attribute))
            {
                attribute.SetBaseValue(value);
            }
        }

        public int GetMaxAttributeValue(AttributeType type)
        {
            Attribute attribute;
            if (Attributes.TryGetValue(type, out attribute))
            {
                return attribute.GetMaxValue();
            }
            return 0;
        }

        public void ModifyAttributeValue(AttributeType type, int amount)
        {
            Attribute attribute;
            if (Attributes.TryGetValue(type, out attribute))
            {
                attribute.ModifyValue(amount);
            }
        }

        public void ModifyMaxAttributeValue(AttributeType type, int amount)
        {
            Attribute attribute;
            if (Attributes.TryGetValue(type, out attribute))
            {
                attribute.ModifyMaxValue(amount);
            }
        }

        public void RegisterAttributeModifier(AttributeType type, int value)
        {
            Attribute attribute;
            if (Attributes.TryGetValue(type, out attribute))
            {
                attribute.RegisterBaseAttributeModifier(value);
            }
        }

        public void RegisterMaxAttributeModifier(AttributeType type, int value)
        {
            Attribute attribute;
            if (Attributes.TryGetValue(type, out attribute))
            {
                attribute.RegisterMaxAttributeModifier(value);
            }
        }

        public void DeregisterAttributeModifier(AttributeType type, int value)
        {
            Attribute attribute;
            if (Attributes.TryGetValue(type, out attribute))
            {
                attribute.DeregisterBaseAttributeModifier(value);
            }
        }

        public void DeregisterMaxAttributeModifier(AttributeType type, int value)
        {
            Attribute attribute;
            if (Attributes.TryGetValue(type, out attribute))
            {
                attribute.DeregisterMaxAttributeModifier(value);
            }
        }
    }
}