using System;
using Project.Attributes;
using Project.Items;
using UnityEngine;

namespace Project.Combat
{
    public class Combatant
    {
        public CharacterAttributes Attributes;
        public string DisplayName;
        public string Description;
        public Sprite Sprite;
        public Inventory Inventory;

        public Combatant(CharacterAttributes attributes, string displayName, string description, Sprite sprite, Inventory inventory)
        {
            this.Attributes = attributes;
            this.DisplayName = displayName;
            this.Description = description;
            this.Sprite = sprite;
            this.Inventory = inventory;
        }

        public void Attack(out int value)
        {
            value = Attributes.GetAttributeValue(AttributeType.Strength);
        }

        public void ReceiveAttack(HitReport hitReport)
        {
            Attributes.ModifyAttributeValue(AttributeType.Health, -Math.Abs(hitReport.Damage));
        }
    }
}