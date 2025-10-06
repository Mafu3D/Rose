using System.Collections.Generic;

namespace Project.Combat.StatusEffects
{
    public enum StatusEffectType
    {
        Frost,
        Frozen,
        Burn,
        Weakened,
        Vulnerable
    }

    public class StatusEffectManager
    {
        Dictionary<int, StatusEffect> registeredStatusEffects = new();

        public Dictionary<int, StatusEffect> GetRegisteredStatusEffects() => registeredStatusEffects;

        public void AddStack(StatusEffect statusEffect, int amount)
        {
            StatusEffect registeredStatusEffect = GetOrAddStatusEffect(statusEffect);
            registeredStatusEffect.AddStacks(amount);
        }

        public void RemoveStack(StatusEffect statusEffect, int amount)
        {
            StatusEffect registeredStatusEffect = GetOrAddStatusEffect(statusEffect);
            registeredStatusEffect.RemoveStacks(amount);
            if (registeredStatusEffect.Stacks <= 0)
            {
                RemoveStatusEffect(registeredStatusEffect);
            }
        }

        public void RemoveStatusEffect(StatusEffect statusEffect)
        {
            statusEffect.OnAllStacksRemovedEvent -= RemoveStatusEffect;
            StatusEffect registeredStatusEffect = GetOrAddStatusEffect(statusEffect);
            registeredStatusEffects.Remove(registeredStatusEffect.Hash);
        }

        public void ClearAllStatusEffects()
        {
            Dictionary<int, StatusEffect> effectsToRemove = new(registeredStatusEffects);
            foreach (KeyValuePair<int, StatusEffect> keyValuePair in effectsToRemove)
            {
                RemoveStatusEffect(keyValuePair.Value);
            }
        }

        public List<StatusEffect> GetAllStatusEffects()
        {
            List<StatusEffect> allEffects = new();
            foreach (KeyValuePair<int, StatusEffect> keyValuePair in registeredStatusEffects)
            {
                allEffects.Add(keyValuePair.Value);
            }
            return allEffects;
        }

        private StatusEffect GetOrAddStatusEffect(StatusEffect toAdd)
        {
            StatusEffect registered;
            if (registeredStatusEffects.TryGetValue(toAdd.Hash, out registered))
            {
                return registered;
            }
            registeredStatusEffects.Add(toAdd.Hash, toAdd);
            toAdd.OnAllStacksRemovedEvent += RemoveStatusEffect;
            return toAdd;
        }
    }
}