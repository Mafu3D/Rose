using System;
using Project.Attributes;
using Project.Combat;
using Project.Items;
using UnityEngine;

namespace Project
{
    public class Character
    {
        public CharacterAttributes Attributes;
        public string DisplayName;
        public string Description;
        public string CombatDescription;
        public Sprite Sprite;
        public Inventory Inventory;

        public Character(CharacterData characterData, Inventory inventory)
        {
            this.Attributes = new CharacterAttributes(characterData.AttributesData);
            this.DisplayName = characterData.DisplayName;
            this.Description = characterData.Description;
            this.CombatDescription = characterData.CombatDescription;
            this.Sprite = characterData.CombatSprite;
            this.Inventory = inventory;
        }

        public int GetAttackValue()
        {
            return Attributes.GetAttributeValue(AttributeType.Strength);
        }

        public void ReceiveAttack(HitReport hitReport)
        {
            Attributes.ModifyAttributeValue(AttributeType.Health, -Math.Abs(hitReport.Damage));
        }
    }
}