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
            if (targetCharacter == CombatActionTarget.User) target = user;

            switch (effectType)
            {
                case StatusEffectType.Frost:
                    if (!target.CharacterData.ImmuneToFrost)
                    {
                        FrostStatusEffect frost = new FrostStatusEffect(target, user, 3);
                        target.StatusEffectManager.AddStack(frost, stackAmount);
                    }
                    break;
                case StatusEffectType.Burn:
                    if (!target.CharacterData.ImmuneToBurn)
                    {
                        BurnStatusEffect burn = new BurnStatusEffect(target, user, 99);
                        target.StatusEffectManager.AddStack(burn, stackAmount);
                    }
                    break;
                case StatusEffectType.Weakened:
                    if (!target.CharacterData.ImmuneToWeaken)
                    {
                        WeakenStatusEffect weak = new WeakenStatusEffect(target, user, 99);
                        target.StatusEffectManager.AddStack(weak, stackAmount);
                    }
                    break;
                case StatusEffectType.Vulnerable:
                    if (!target.CharacterData.ImmuneToVulnerable)
                    {
                        VulnerableStatusEffect vulnerable = new VulnerableStatusEffect(target, user, 99);
                        target.StatusEffectManager.AddStack(vulnerable, stackAmount);
                    }
                    break;
            }
        }

        protected override string Message(Character user, Character target)
        {
            if (targetCharacter == CombatActionTarget.User) target = user;

            string stackOrStacks;
            if (stackAmount == 1) stackOrStacks = "stack";
            else stackOrStacks = "stacks";

            return $"{target.DisplayName} received {stackAmount} {stackOrStacks} of {effectType.ToString()}";
        }
    }
}