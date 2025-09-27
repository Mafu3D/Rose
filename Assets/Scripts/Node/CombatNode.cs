using System;
using Project.Combat;
using Project.Attributes;
using UnityEngine;

namespace Project.GameNode
{
    public class CombatNode : Node
    {
        [SerializeField] public AttributesData attributesData;
        public CharacterAttributes Attributes;

        protected override void Awake()
        {
            base.Awake();
            Attributes = new CharacterAttributes(attributesData);
        }

        public override Status Resolve()
        {
            // Noop
            return Status.Running;
        }

        public override void Reset()
        {
            // Noop
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