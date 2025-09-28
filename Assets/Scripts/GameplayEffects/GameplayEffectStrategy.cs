using Project.GameNode;
using UnityEngine;

namespace Project.GameplayEffects
{
    public abstract class GameplayEffectStrategy : ScriptableObject
    {
        public abstract Status Start();
        public abstract Status Resolve();
        public abstract void Reset();
    }
}