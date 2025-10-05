using Project.Combat;
using UnityEngine;

namespace Project.Items
{
    public abstract class CombatEffectStrategy : ScriptableObject
    {
        public abstract Status StartEffect(Character user, Character target);
        public abstract Status ResolveEffect(Character user, Character target);
        public abstract void ResetEffect(Character user, Character target);
        public abstract void EndEffect(Character user, Character target);
    }
}