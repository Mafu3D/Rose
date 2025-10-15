using System;
using System.Collections.Generic;
using Project.Combat.StatusEffects;
using UnityEngine;

namespace Project.Combat.CombatActions
{
    public enum ConditionalOperand
    {
        FrostStacks,
        BurnStacks,
        WeakenedStacks,
        VulnerableStacks,
        Exposed,
        Bloodied,
        Frozen,
        Health,
        Armor,
        Strength,
        Speed,
        Multistrike
    }

    public enum Operation
    {
        LessThan,
        LessThanOrEqualTo,
        EqualTo,
        GreaterThan,
        GreaterThanOrEqualTo
    }

    [CreateAssetMenu(fileName = "NewConditional", menuName = "Combat Actions/Conditional", order = 1)]
    public class Conditional : CombatActionBaseData
    {
        [SerializeField] CombatActionTarget targetCharacter;
        [SerializeField] ConditionalOperand toCheck;
        [SerializeField] Operation operation;
        [SerializeField] int value;
        [SerializeField] List<CombatActionBaseData> actionDatas;

        public override void Execute(Character user, Character target)
        {
            if (targetCharacter == CombatActionTarget.User) target = user;
            int firstOperand = GetFirstOperand(target);
            Predicate<int> predicate = GetOperationPredicate(firstOperand);

            if (predicate(value))
            {
                foreach (CombatActionBaseData actionData in actionDatas)
                {
                    actionData.QueueAction(GameManager.Instance.BattleManager.ActiveBattle.CombatQueue, user, target);
                }
            }
        }

        protected override string Message(Character user, Character target)
        {
            string message = "";
            return message;
        }

        private Predicate<int> GetOperationPredicate(int firstOperand)
        {
            switch (operation)
            {
                case Operation.LessThan:
                    return (int value) => { return firstOperand < value; };
                case Operation.LessThanOrEqualTo:
                    return (int value) => { return firstOperand <= value; };
                case Operation.EqualTo:
                    return (int value) => { return firstOperand == value; };
                case Operation.GreaterThan:
                    return (int value) => { return firstOperand > value; };
                case Operation.GreaterThanOrEqualTo:
                    return (int value) => { return firstOperand >= value; };
            }
            return (int value) => { return false; };
        }

        private int GetFirstOperand(Character character)
        {
            int stacks = 0;
            switch (toCheck)
            {
                case ConditionalOperand.FrostStacks:
                    character.StatusEffectManager.TryGetStacksOf<FrostStatusEffect>(out stacks);
                    return stacks;
                case ConditionalOperand.Frozen:
                    character.StatusEffectManager.TryGetStacksOf<FrozenStatusEffect>(out stacks);
                    return stacks;
                case ConditionalOperand.BurnStacks:
                    character.StatusEffectManager.TryGetStacksOf<BurnStatusEffect>(out stacks);
                    return stacks;
                case ConditionalOperand.WeakenedStacks:
                    character.StatusEffectManager.TryGetStacksOf<WeakenStatusEffect>(out stacks);
                    return stacks;
                case ConditionalOperand.VulnerableStacks:
                    character.StatusEffectManager.TryGetStacksOf<VulnerableStatusEffect>(out stacks);
                    return stacks;
                case ConditionalOperand.Exposed:
                    if (character.Attributes.GetAttributeValue(Attributes.AttributeType.Armor) <= 0)
                    {
                        return 1;
                    }
                    return 0;
                case ConditionalOperand.Bloodied:
                    if (character.Attributes.GetAttributeValue(Attributes.AttributeType.Health) <= character.Attributes.GetMaxAttributeValue(Attributes.AttributeType.Health) / 2)
                    {
                        return 1;
                    }
                    return 0;
                case ConditionalOperand.Health:
                    return character.Attributes.GetAttributeValue(Attributes.AttributeType.Health);
                case ConditionalOperand.Armor:
                    return character.Attributes.GetAttributeValue(Attributes.AttributeType.Armor);
                case ConditionalOperand.Strength:
                    return character.Attributes.GetAttributeValue(Attributes.AttributeType.Strength);
                case ConditionalOperand.Speed:
                    return character.Attributes.GetAttributeValue(Attributes.AttributeType.Speed);
                case ConditionalOperand.Multistrike:
                    return character.Attributes.GetAttributeValue(Attributes.AttributeType.Multistrike);
            }
            return 0;
        }
    }
}