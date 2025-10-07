using Project.Attributes;
using UnityEngine;

namespace Project.Combat.CombatActions
{


    [CreateAssetMenu(fileName = "NewModifyAttribute", menuName = "Combat Actions/Modify Attribute", order = 1)]
    public class ModifyUserAttribute : CombatActionBaseData
    {
        [SerializeField] bool IsPermanent = false;
        [SerializeField] CombatActionTarget targetCharacter;
        [SerializeField] AttributeType AttributeType;
        [SerializeField] int BaseValueModifier;
        [SerializeField] int MaxValueModifier;

        public override void Execute(Character user, Character target)
        {
            Character character;
            if (targetCharacter == CombatActionTarget.User)
            {
                character = user;
            }
            else
            {
                character = target;
            }

            if (AttributeType == AttributeType.Health || AttributeType == AttributeType.Armor)
            {
                if (MaxValueModifier != 0)
                {
                    if (IsPermanent) { character.StoredAttributes.ModifyMaxAttributeValue(AttributeType, MaxValueModifier); }
                    character.Attributes.ModifyMaxAttributeValue(AttributeType, MaxValueModifier);
                }

                if (BaseValueModifier != 0)
                {
                    if (IsPermanent) { character.StoredAttributes.ModifyAttributeValue(AttributeType, BaseValueModifier); }
                    character.Attributes.ModifyAttributeValue(AttributeType, BaseValueModifier);
                }
            }
            else
            {
                if (MaxValueModifier != 0)
                {
                    if (IsPermanent) { character.StoredAttributes.RegisterMaxAttributeModifier(AttributeType, MaxValueModifier); }
                    character.Attributes.RegisterMaxAttributeModifier(AttributeType, MaxValueModifier);
                }

                if (BaseValueModifier != 0)
                {
                    if (IsPermanent) { character.StoredAttributes.RegisterAttributeModifier(AttributeType, BaseValueModifier); }
                    character.Attributes.RegisterAttributeModifier(AttributeType, BaseValueModifier);
                }
            }
        }

        protected override string Message(Character user, Character target)
        {
            string message = "";
            if (MaxValueModifier != 0)
            {
                string incOrDec;
                if (MaxValueModifier > 0) incOrDec = "increased";
                else incOrDec = "decreased";

                Character character;
                if (targetCharacter == CombatActionTarget.User) character = user;
                else character = target;

                message += $"{user.DisplayName} {incOrDec} {character.DisplayName}'s Max {AttributeType.ToString()} by {MaxValueModifier}";
            }

            if (BaseValueModifier != 0)
            {
                string incOrDec;
                if (BaseValueModifier > 0) incOrDec = "increased";
                else incOrDec = "decreased";

                Character character;
                if (targetCharacter == CombatActionTarget.User) character = user;
                else character = target;

                message += $"{user.DisplayName} {incOrDec} {character.DisplayName}'s {AttributeType.ToString()} by {BaseValueModifier}";
            }

            return message;
        }
    }
}