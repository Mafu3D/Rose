using System;

namespace Project.Combat.StatusEffects
{
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
        public abstract void OnHit();
        public abstract void OnReceiveHit();
        public abstract void OnTurnStart();
        public abstract void OnTurnEnd();
        public abstract void OnRoundStart();
        public abstract void OnRoundEnd();
        public abstract void OnEnemyTurnStart();  // Not implemented into battle yet
        public abstract void OnEnemyTurnEnd();  // Not implemented into battle yet
        public abstract void OnSelfBloodied();  // Not implemented into battle yet
        public abstract void OnEnemyBloodied();  // Not implemented into battle yet
        public abstract void OnSelfExposed();  // Not implemented into battle yet
        public abstract void OnEnemyExposed();  // Not implemented into battle yet
    }
}