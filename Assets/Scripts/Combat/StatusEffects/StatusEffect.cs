using System;
using Project.Combat.CombatActions;

namespace Project.Combat.StatusEffects
{
    public enum StatusEffectEventName {

    }

    public abstract class StatusEffect
    {
        public abstract string DisplayName { get; }
        public int Hash => DisplayName.ToFNV1aHash();
        public int Stacks;
        public int MaxStacks;
        public event Action<StatusEffect> OnAllStacksRemovedEvent;
        protected Character owner;
        protected Character enemy;

        public StatusEffect(Character owner, Character enemy, int maxStacks)
        {
            this.owner = owner;
            this.enemy = enemy;
            this.MaxStacks = maxStacks;
        }

        public void AddStacks(int amount)
        {
            if (amount != 0)
            {
                Stacks = Math.Clamp(Stacks + Math.Abs(amount), 0, MaxStacks);
                OnReceiveNewStack();

                if (Stacks >= MaxStacks)
                {
                    OnReceiveMaxStacks();
                }
            }
        }
        public void RemoveStacks(int amount)
        {
            if (amount != 0)
            {
                Stacks = Math.Clamp(Stacks - Math.Abs(amount), 0, MaxStacks);
                OnRemoveStack();

                if (Stacks <= 0)
                {
                    OnAllStacksRemoved();
                    OnAllStacksRemovedEvent?.Invoke(this);
                }
            }
        }

        // Stack based actions
        public abstract void OnReceiveNewStack();
        public abstract void OnRemoveStack();
        public abstract void OnReceiveMaxStacks();
        public abstract void OnAllStacksRemoved();

        // Combat actions
        public abstract CombatAction OnHit();
        public abstract CombatAction OnReceiveHit();
        public abstract CombatAction OnTurnStart();
        public abstract CombatAction OnTurnEnd();
        public abstract CombatAction OnRoundStart();
        public abstract CombatAction OnRoundEnd();
        public abstract CombatAction OnEnemyTurnStart();  // Not implemented into battle yet
        public abstract CombatAction OnEnemyTurnEnd();  // Not implemented into battle yet
        public abstract CombatAction OnSelfBloodied();  // Not implemented into battle yet
        public abstract CombatAction OnEnemyBloodied();  // Not implemented into battle yet
        public abstract CombatAction OnSelfExposed();  // Not implemented into battle yet
        public abstract CombatAction OnEnemyExposed();  // Not implemented into battle yet
    }
}