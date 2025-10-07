using Project.Combat.CombatActions;
using Project.VFX;

namespace Project.Combat.StatusEffects
{
    public class VulnerableStatusEffect : StatusEffect
    {
        public override string DisplayName => "Vulnerable";
        public VulnerableStatusEffect(Character owner, Character enemy, int maxStacks) : base(owner, enemy, maxStacks) { }

        public override void OnAllStacksRemoved() { }
        public override void OnReceiveMaxStacks() { }

        public override void OnReceiveNewStack(int amount)
        {
            VFXManager.Instance.PlayStatusEffectVFX("VulnerableOnApply", owner);
        }

        public override void OnRemoveStack(int amount) { }

        public override CombatAction OnHit() { return null; }

        public override CombatAction OnReceiveHit() {
            return new CombatAction(() =>
            {
                owner.TakeDamage(Stacks);
                VFXManager.Instance.PlayStatusEffectVFX("VulnerableOnTrigger", owner);
            },
            $"{owner.DisplayName} took an additional {Stacks} damage"); }

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