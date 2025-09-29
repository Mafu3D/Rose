using System.Collections.Generic;
using Project.Attributes;
using Project.GameNode;
using Project.GameNode.Hero;
using UnityEngine;

namespace Project.GameplayEffects
{
    [CreateAssetMenu(fileName = "NewModifyAttribute", menuName = "Effects/Modify Attribute", order = 1)]
    public class ModifyAttribute : GameplayEffectStrategy
    {
        [SerializeField] AttributeType AttributeType;
        [SerializeField] int BaseValueModifier;
        [SerializeField] int MaxValueModifier;

        public override void ResetEffect(Node user, Node target) { }
        public override Status ResolveEffect(Node user, Node target) { return Status.Complete; }

        public override Status StartEffect(Node user, Node target)
        {
            HeroNode heroNode = GameManager.Instance.Player.HeroNode;
            if (AttributeType == AttributeType.Health)
            {
                if (MaxValueModifier != 0)
                {
                    heroNode.Attributes.ModifyMaxAttributeValue(AttributeType, MaxValueModifier);
                }

                if (BaseValueModifier != 0)
                {
                    heroNode.Attributes.ModifyAttributeValue(AttributeType, BaseValueModifier);
                }
            }
            else
            {
                if (MaxValueModifier != 0)
                {
                    heroNode.Attributes.RegisterMaxAttributeModifier(AttributeType, MaxValueModifier);
                }

                if (BaseValueModifier != 0)
                {
                    heroNode.Attributes.RegisterAttributeModifier(AttributeType, BaseValueModifier);
                }
            }


            return Status.Complete;
        }
    }
}