using System;

namespace Project.Combat.CombatActions
{
    public class CombatAction
    {
        Combatant User;
        Combatant Target;
        Action<Combatant, Combatant> ExecuteAction;

        public CombatAction(Combatant user, Combatant target, Action<Combatant, Combatant> executeAction)
        {
            this.User = user;
            this.Target = target;
            this.ExecuteAction = executeAction;
        }

        public void Execute() => ExecuteAction(User, Target);
    }
}