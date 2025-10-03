using UnityEngine;

namespace Project.Combat.CombatActions
{
    public abstract class CombatActionBaseData : ScriptableObject
    {
        public void QueueAction(CombatQueue queue, Combatant user, Combatant target)
        {
            CombatAction combatAction = new CombatAction(user, target, Execute);
            queue.AddAction(combatAction);
        }

        public abstract void Execute(Combatant user, Combatant target);
    }
}