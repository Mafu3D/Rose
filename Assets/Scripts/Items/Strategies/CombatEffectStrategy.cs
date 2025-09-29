using Project.Combat;
using UnityEngine;

namespace Project.Items
{
    public abstract class CombatEffectStrategy : ScriptableObject
    {
        public abstract Status StartEffect(Combatant user, Combatant target);
        public abstract Status ResolveEffect(Combatant user, Combatant target);
        public abstract void ResetEffect(Combatant user, Combatant target);
        public abstract void EndEffect(Combatant user, Combatant target);
    }
}