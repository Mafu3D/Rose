using UnityEngine;

namespace Project.Combat.CombatActions
{
    [CreateAssetMenu(fileName = "NewDealDamage", menuName = "Combat Actions/Deal Damage", order = 1)]
    public class DealDamage : CombatActionBaseData
    {
        [SerializeField] CombatActionTarget targetCharacter;
        [SerializeField] int amount;

        public override void Execute(Character user, Character target)
        {
            if (targetCharacter == CombatActionTarget.User) target = user;
            HitReport hitReport = new HitReport(amount);
            target.ReceiveAttack(hitReport);
        }

        protected override string Message(Character user, Character target)
        {
            if (targetCharacter == CombatActionTarget.User) target = user;
            return $"{user.DisplayName} dealt {amount} extra damage to {target.DisplayName}";
        }
    }
}