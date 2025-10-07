using System;
using UnityEngine;

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

        Action ExecuteStatusEffect;
        public string Message;

        public CombatAction(Character user, Character target, Action<Character, Character> executeAction, string message)
        {
            this.User = user;
            this.Target = target;
            this.ExecuteAction = executeAction;
            this.Message = message;
        }

        public CombatAction(Action executeStatusEffect, string message)
        {
            this.ExecuteStatusEffect = executeStatusEffect;
            this.Message = message;
        }

        public void Execute()
        {
            if (ExecuteAction != null)
            {
                ExecuteAction(User, Target);
            }
            else if (ExecuteStatusEffect != null)
            {
                ExecuteStatusEffect();
            }
        }
    }
}