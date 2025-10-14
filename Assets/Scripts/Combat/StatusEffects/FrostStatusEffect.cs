using Project.Attributes;
using Project.Combat.CombatActions;
using Project.VFX;
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
            FrozenStatusEffect frozenStatusEffect = new FrozenStatusEffect(owner, source, 99);
            owner.StatusEffectManager.AddStack(frozenStatusEffect, 1);
            RemoveStacks(Stacks);

        }

        public override void OnReceiveNewStack(int amount)
        {
            owner.Attributes.ModifyAttributeValue(AttributeType.Speed, -1);
            VFXManager.Instance.PlayStatusEffectVFX("FrostOnApply", owner);
        }

        public override void OnRemoveStack(int amount) { }

        public override CombatAction OnHit() { return null; }

        public override CombatAction OnReceiveHit() { return null; }

        public override CombatAction OnTurnEnd() { return null; }

        public override CombatAction OnTurnStart() { return null; }

        public override CombatAction OnRoundStart() { return null; }

        public override CombatAction OnRoundEnd() { return null; }

        public override CombatAction OnEnemyTurnStart() { return null; }

        public override CombatAction OnEnemyTurnEnd() { return null; }

        public override CombatAction OnSelfBloodied() { return null; }

        public override CombatAction OnEnemyBloodied() { return null; }

        public override CombatAction OnSelfExposed() { return null; }

        public override CombatAction OnEnemyExposed() { return null; }
    }
}