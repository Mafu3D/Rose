using UnityEngine;

namespace Project.Combat.CombatActions
{
    public abstract class CombatActionBaseData : ScriptableObject
    {
        public void QueueAction(CombatQueue queue, Character user, Character target)
        {
            CombatAction combatAction = new CombatAction(user, target, Execute);
            queue.AddAction(combatAction);
        }

        public abstract void Execute(Character user, Character target);
    }
}