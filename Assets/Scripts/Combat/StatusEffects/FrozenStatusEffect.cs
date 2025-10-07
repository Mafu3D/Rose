using Project.Attributes;
using Project.Combat.CombatActions;
using Project.VFX;
using UnityEngine;

namespace Project.Combat.StatusEffects
{
    public class FrozenStatusEffect : StatusEffect
    {
        public override string DisplayName => "Frozen";
        public FrozenStatusEffect(Character owner, Character enemy, int maxStacks) : base(owner, enemy, maxStacks) { }

        public override void OnAllStacksRemoved() { }
        public override void OnReceiveMaxStacks() { }

        public override void OnReceiveNewStack(int amount)
        {
            owner.Stunned += 1;
            VFXManager.Instance.PlayStatusEffectVFX("FrozenOnApply", owner);
        }

        public override void OnRemoveStack(int amount) { }

        public override CombatAction OnHit() { return null; }

        public override CombatAction OnReceiveHit() { return null; }


        public override CombatAction OnTurnEnd() { return null; }

        public override CombatAction OnTurnStart() { return null; }

        public override CombatAction OnRoundStart() { return null; }

        public override CombatAction OnRoundEnd()
        {
            int amount = 1;
            return new CombatAction(() => RemoveStacks(amount), $"{amount} Frozen was removed from {owner.DisplayName}");
        }

        public override CombatAction OnEnemyTurnStart() { return null; }

        public override CombatAction OnEnemyTurnEnd() { return null; }

        public override CombatAction OnSelfBloodied() { return null; }

        public override CombatAction OnEnemyBloodied() { return null; }

        public override CombatAction OnSelfExposed() { return null; }

        public override CombatAction OnEnemyExposed() { return null; }
    }
}