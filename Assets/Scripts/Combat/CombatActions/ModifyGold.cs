using System;
using UnityEngine;

namespace Project.Combat.CombatActions
{
    [CreateAssetMenu(fileName = "NewModifyGold", menuName = "Combat Actions/Modify Gold", order = 1)]
    public class ModifyGold : CombatActionBaseData
    {
        [SerializeField] int amount;

        public override void Execute(Character user, Character target)
        {
            Debug.Log("adding gold!");
            if (amount > 0)
            {
                GameManager.Instance.Player.GoldTracker.AddGold(Math.Abs(amount));
            }
            else if (amount < 0)
            {
                GameManager.Instance.Player.GoldTracker.RemoveGold(Math.Abs(amount));
            }
        }

        protected override string Message(Character user, Character target)
        {
            string message;
            if (amount >= 0)
            {
                message = $"Gained {Math.Abs(amount)} gold";
            }
            else
            {
                message = $"Lost {Math.Abs(amount)} gold";
            }
            return message;
        }
    }
}