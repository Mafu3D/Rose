using Project.Combat.StatusEffects;
using UnityEngine;

namespace Project.Combat.CombatActions
{
    [CreateAssetMenu(fileName = "NewAddStatusEffect", menuName = "Combat Actions/Add Status Effect", order = 1)]
    public class AddStatusEffect : CombatActionBaseData
    {
        [SerializeField] CombatActionTarget targetCharacter;
        [SerializeField] StatusEffectType effectType;
        [SerializeField] int stackAmount;

        public override void Execute(Character user, Character target)
        {
            Character character;
            Character other;
            if (targetCharacter == CombatActionTarget.User)
            {
                character = user;
                other = target;
            }
            else
            {
                character = target;
                other = user;
            }

            switch (effectType)
            {
                case StatusEffectType.Frost:
                    FrostStatusEffect frost = new FrostStatusEffect(character, other, 3);
                    character.StatusEffectManager.AddStack(frost, stackAmount);
                    break;
            }
        }

        protected override string Message(Character user, Character target)
        {
            Character character;
            if (targetCharacter == CombatActionTarget.User) character = user;
            else character = target;

            string stackOrStacks;
            if (stackAmount == 1) stackOrStacks = "stack";
            else stackOrStacks = "stacks";

            return $"{character.DisplayName} received {stackAmount} {stackOrStacks} of {effectType.ToString()}";
        }
    }
}