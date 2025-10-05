using Project.Attributes;
using UnityEngine;

namespace Project.Combat.StatusEffects
{
    public class FrostStatusEffect : StatusEffect
    {
        public override string DisplayName => "Frost";
        public FrostStatusEffect(Character owner, Character enemy, int maxStacks) : base(owner, enemy, maxStacks) { }

        public override void OnAllStacksRemoved() { }
        public override void OnReceiveMaxStacks()
        {
            FrozenStatusEffect frozenStatusEffect = new FrozenStatusEffect(owner, enemy, 99);
            owner.StatusEffectManager.AddStack(frozenStatusEffect, 1);
            RemoveStacks(Stacks);
        }

        public override void OnReceiveNewStack()
        {
            owner.Attributes.ModifyAttributeValue(AttributeType.Speed, -1);
        }

        public override void OnRemoveStack() { }

        public override void OnHit()
        {
            Debug.Log($"{owner.DisplayName} was hit while having {Stacks} stacks of Frost!");
        }

        public override void OnReceiveHit() { }


        public override void OnTurnEnd() { }

        public override void OnTurnStart() { }

        public override void OnRoundStart() { }

        public override void OnRoundEnd() { }

        public override void OnEnemyTurnStart() { }

        public override void OnEnemyTurnEnd() { }

        public override void OnSelfBloodied() { }

        public override void OnEnemyBloodied() { }

        public override void OnSelfExposed() { }

        public override void OnEnemyExposed() { }
    }
}