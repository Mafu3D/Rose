using System.Collections.Generic;
using Project.Attributes;
using Project.Combat;
using Project.GameNode;
using Project.GameNode.Hero;
using Project.Items;
using Project.States;
using UnityEngine;

namespace Project.CombatEffects
{
    [CreateAssetMenu(fileName = "NewCombatModifyAttribute", menuName = "Combat Effects/Modify Attribute", order = 1)]
    public class ModifyAttribute : CombatEffectStrategy
    {
        [SerializeField] AttributeType AttributeType;
        [SerializeField] int BaseValueModifier;
        [SerializeField] int MaxValueModifier;

        public override void EndEffect(Combatant user, Combatant target)
        {
        }

        public override void ResetEffect(Combatant user, Combatant target)
        {
        }


        public override Status ResolveEffect(Combatant user, Combatant target)
        {
            return Status.Complete;
        }

        public override Status StartEffect(Combatant user, Combatant target)
        {
            if (AttributeType == AttributeType.Health)
            {
                if (MaxValueModifier != 0)
                {
                    user.Attributes.ModifyMaxAttributeValue(AttributeType, MaxValueModifier);
                }

                if (BaseValueModifier != 0)
                {
                    user.Attributes.ModifyAttributeValue(AttributeType, BaseValueModifier);
                }
            }
            else
            {
                if (MaxValueModifier != 0)
                {
                    user.Attributes.RegisterMaxAttributeModifier(AttributeType, MaxValueModifier);
                }

                if (BaseValueModifier != 0)
                {
                    user.Attributes.RegisterAttributeModifier(AttributeType, BaseValueModifier);
                }
            }


            return Status.Complete;
        }
    }
}