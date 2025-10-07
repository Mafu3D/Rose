using Project.Attributes;
using Project.Combat.CombatActions;
using Project.VFX;

namespace Project.Combat.StatusEffects
{
    public class WeakenStatusEffect : StatusEffect
    {
        public override string DisplayName => "Weak";
        public WeakenStatusEffect(Character owner, Character enemy, int maxStacks) : base(owner, enemy, maxStacks) { }

        public override void OnAllStacksRemoved() { }
        public override void OnReceiveMaxStacks() { }

        public override void OnReceiveNewStack(int amount)
        {
            owner.Attributes.ModifyAttributeValue(AttributeType.Strength, -amount);
            VFXManager.Instance.PlayStatusEffectVFX("WeakOnApply", owner);
        }

        public override void OnRemoveStack(int amount)
        {
            owner.Attributes.ModifyAttributeValue(AttributeType.Strength, amount);
        }

        public override CombatAction OnHit() { return new CombatAction(() => RemoveAllStacks(), $"{DisplayName} was removed from {owner.DisplayName}"); }

        public override CombatAction OnReceiveHit() { return null; }

        public override CombatAction OnTurnEnd()
        {
            return null;
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