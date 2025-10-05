using Project.Attributes;
using UnityEngine;

namespace Project.Combat.StatusEffects
{
    public class FrozenStatusEffect : StatusEffect
    {
        public override string DisplayName => "Frozen";
        public FrozenStatusEffect(Character owner, Character enemy, int maxStacks) : base(owner, enemy, maxStacks) { }

        public override void OnAllStacksRemoved() { }
        public override void OnReceiveMaxStacks() { }

        public override void OnReceiveNewStack()
        {
            owner.Stunned += 1;
        }

        public override void OnRemoveStack() { }

        public override void OnHit() { }

        public override void OnReceiveHit() { }


        public override void OnTurnEnd() { }

        public override void OnTurnStart() { }

        public override void OnRoundStart() { }

        public override void OnRoundEnd()
        {
            RemoveStacks(1);
        }

        public override void OnEnemyTurnStart() { }

        public override void OnEnemyTurnEnd() { }

        public override void OnSelfBloodied() { }

        public override void OnEnemyBloodied() { }

        public override void OnSelfExposed() { }

        public override void OnEnemyExposed() { }
    }
}