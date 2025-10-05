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
        public CharacterAttributes StoredAttributes;
        public string DisplayName;
        public string Description;
        public string CombatDescription;
        public Sprite Sprite;
        public Inventory Inventory;

        int shapshotArmorValue;

        public Character(CharacterData characterData, Inventory inventory)
        {
            this.Attributes = new CharacterAttributes(characterData.AttributesData);
            this.DisplayName = characterData.DisplayName;
            this.Description = characterData.Description;
            this.CombatDescription = characterData.CombatDescription;
            this.Sprite = characterData.CombatSprite;
            this.Inventory = inventory;
        }

        // public void SnapshotArmorValue()
        // {
        //     shapshotArmorValue = Attributes.GetAttributeBaseValue(AttributeType.Armor);
        // }

        // public void RestoreSnapshotArmorvalue()
        // {
        //     Attributes.
        // }

        public int GetAttackValue()
        {
            return Attributes.GetAttributeValue(AttributeType.Strength);
        }

        public void ReceiveAttack(HitReport hitReport)
        {
            int armor = Attributes.GetAttributeValue(AttributeType.Armor);
            Attributes.ModifyAttributeValue(AttributeType.Armor, -Math.Abs(hitReport.Damage));
            int carryOverDamage = armor - hitReport.Damage;
            if (carryOverDamage < 0)
            {
                carryOverDamage = Math.Abs(carryOverDamage);
                Attributes.ModifyAttributeValue(AttributeType.Health, -carryOverDamage);
            }
        }

        public void ShapshotAttributes()
        {
            StoredAttributes = Attributes.Copy();
        }

        public void RestoreSnapshotAttributes()
        {
            StoredAttributes.ReplaceAttribute(AttributeType.Health, Attributes.GetAttribute(AttributeType.Health));
            Attributes = StoredAttributes;
        }
    }
}