using Project.Combat.CombatActions;
using Project.VFX;

namespace Project.Combat.StatusEffects
{
    public class BurnStatusEffect : StatusEffect
    {
        public override string DisplayName => "Burn";
        public BurnStatusEffect(Character owner, Character enemy, int maxStacks) : base(owner, enemy, maxStacks) { }

        public override void OnAllStacksRemoved() { }
        public override void OnReceiveMaxStacks() { }

        public override void OnReceiveNewStack(int amount)
        {
            VFXManager.Instance.PlayStatusEffectVFX("BurnOnApply", owner);
        }

        public override void OnRemoveStack(int amount) { }

        public override CombatAction OnHit() { return null; }

        public override CombatAction OnReceiveHit() { return null; }

        public override CombatAction OnTurnEnd()
        {
            int startingStacks = Stacks;
            return new CombatAction(() =>
            {
                owner.TakeDamage(startingStacks);
                VFXManager.Instance.PlayStatusEffectVFX("BurnOnTrigger", owner);
                RemoveStacks(1);
            },
            $"{owner.DisplayName} was burnt for {startingStacks} damage");
        }

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