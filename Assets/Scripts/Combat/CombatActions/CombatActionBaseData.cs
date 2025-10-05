using UnityEngine;

namespace Project.Combat.CombatActions
{
    public abstract class CombatActionBaseData : ScriptableObject
    {
        public void QueueAction(CombatQueue queue, Character user, Character target)
        {
            CombatAction combatAction = new CombatAction(user, target, Execute, Message(user, target));
            queue.AddAction(combatAction);
        }

        protected abstract string Message(Character user, Character target);

        public abstract void Execute(Character user, Character target);
    }
}