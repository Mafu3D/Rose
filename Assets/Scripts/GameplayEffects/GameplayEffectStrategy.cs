using Project.GameNode;
using UnityEngine;

namespace Project.GameplayEffects
{
    public abstract class GameplayEffectStrategy : ScriptableObject
    {
        public abstract Status StartEffect(Node user, Node target);
        public abstract Status ResolveEffect(Node user, Node target);
        public abstract void ResetEffect(Node user, Node target);
    }
}