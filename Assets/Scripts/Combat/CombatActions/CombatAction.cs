using System;

namespace Project.Combat.CombatActions
{
    public class CombatAction
    {
        Character User;
        Character Target;
        Action<Character, Character> ExecuteAction;

        public CombatAction(Character user, Character target, Action<Character, Character> executeAction)
        {
            this.User = user;
            this.Target = target;
            this.ExecuteAction = executeAction;
        }

        public void Execute() => ExecuteAction(User, Target);
    }
}