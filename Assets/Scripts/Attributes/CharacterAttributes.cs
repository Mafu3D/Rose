using System.Collections.Generic;
using UnityEngine;

namespace Project.Attributes
{
    public class CharacterAttributes
    {
        Dictionary<AttributeType, Attribute> Attributes = new();

        public CharacterAttributes(AttributesData data)
        {
            Attributes.Add(AttributeType.Health, new Attribute(AttributeType.Health, data.Health));
            Attributes.Add(AttributeType.Armor, new Attribute(AttributeType.Armor, data.Armor));
            Attributes.Add(AttributeType.Strength, new Attribute(AttributeType.Strength, data.Strength));
            Attributes.Add(AttributeType.Magic, new Attribute(AttributeType.Magic, data.Magic));
            Attributes.Add(AttributeType.Dexterity, new Attribute(AttributeType.Dexterity, data.Dexterity));
            Attributes.Add(AttributeType.Speed, new Attribute(AttributeType.Speed, data.Speed));
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

        public void IncreaseAttributeValue(AttributeType type, int amount)
        {
            Attribute attribute;
            if (Attributes.TryGetValue(type, out attribute))
            {
                attribute.IncreaseValue(amount);
            }
        }

        public void DecreaseAttributeValue(AttributeType type, int amount)
        {
            Attribute attribute;
            if (Attributes.TryGetValue(type, out attribute))
            {
                attribute.DecreaseValue(amount);
            }
        }

        public void RegisterAttributeModifier(AttributeType type, int value)
        {
            Attribute attribute;
            if (Attributes.TryGetValue(type, out attribute))
            {
                attribute.RegisterAttributeModifier(value);
            }
        }

        public void DeregisterAttributeModifier(AttributeType type, int value)
        {
            Attribute attribute;
            if (Attributes.TryGetValue(type, out attribute))
            {
                attribute.DeregisterAttributeModifier(value);
            }
        }
    }
}