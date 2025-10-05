using System;

namespace Project.Combat.CombatActions
{
    public enum CombatActionTarget
    {
        User,
        Target
    }

    public class CombatAction
    {
        Character User;
        Character Target;
        Action<Character, Character> ExecuteAction;
        public string Message;

        public CombatAction(Character user, Character target, Action<Character, Character> executeAction, string message)
        {
            this.User = user;
            this.Target = target;
            this.ExecuteAction = executeAction;
            this.Message = message;
        }

        public void Execute() => ExecuteAction(User, Target);
    }
}