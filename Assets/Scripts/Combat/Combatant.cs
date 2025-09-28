using Project.Attributes;
using UnityEngine;

namespace Project.Combat
{
    public class Combatant
    {
        public CharacterAttributes Attributes;
        public string DisplayName;
        public string Description;
        public Sprite Sprite;

        public Combatant(CharacterAttributes attributes, string displayName, string description, Sprite sprite)
        {
            this.Attributes = attributes;
            this.DisplayName = displayName;
            this.Description = description;
            this.Sprite = sprite;

        }

        public void Attack(out int value)
        {
            value = Attributes.GetAttributeValue(AttributeType.Strength);
        }

        public void ReceiveAttack(HitReport hitReport)
        {
            Attributes.DecreaseAttributeValue(AttributeType.Health, hitReport.Damage);
        }
    }
}