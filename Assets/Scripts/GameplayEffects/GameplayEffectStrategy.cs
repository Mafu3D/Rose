using Project.GameTiles;
using UnityEngine;

namespace Project.GameplayEffects
{
    public abstract class GameplayEffectStrategy : ScriptableObject
    {
        public abstract Status StartEffect();
        public abstract Status ResolveEffect();
        public abstract void ResetEffect();
    }
}