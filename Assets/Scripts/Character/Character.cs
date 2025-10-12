using System;
using Project.Attributes;
using Project.Combat;
using Project.Combat.StatusEffects;
using Project.Items;
using UnityEngine;

namespace Project
{
    public class Character
    {
        public CharacterAttributes Attributes;
        public CharacterAttributes StoredAttributes;
        public StatusEffectManager StatusEffectManager { get; private set; }
        public string DisplayName;
        public string Description;
        public string CombatDescription;
        public Sprite Sprite;
        public Inventory Inventory;

        public event Action<HitReport> OnReceiveHit;

        public int Stunned;

        public Character(CharacterData characterData)
        {
            this.Attributes = new CharacterAttributes(characterData.AttributesData);
            this.DisplayName = characterData.DisplayName;
            this.Description = characterData.Description;
            this.CombatDescription = characterData.CombatDescription;
            this.Sprite = characterData.CombatSprite;

            StatusEffectManager = new StatusEffectManager();
        }

        public void SetInventory(Inventory inventory)
        {
            this.Inventory = inventory;
        }

        public int GetAttackValue()
        {
            return Attributes.GetAttributeValue(AttributeType.Strength);
        }

        public void ReceiveAttack(HitReport hitReport)
        {
            TakeDamage(hitReport.Damage);
            OnReceiveHit?.Invoke(hitReport);
        }

        public void TakeDamage(int amount, bool avoidArmor = false)
        {
            int armor = Attributes.GetAttributeValue(AttributeType.Armor);
            Attributes.ModifyAttributeValue(AttributeType.Armor, -Math.Abs(amount));
            int carryOverDamage = armor - amount;
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