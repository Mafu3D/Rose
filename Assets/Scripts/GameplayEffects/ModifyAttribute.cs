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

        public override void Reset() { }
        public override Status Resolve() { return Status.Complete; }

        public override Status Start()
        {
            HeroNode heroNode = GameManager.Instance.Player.HeroNode;
            if (AttributeType == AttributeType.Health)
            {
                if (BaseValueModifier != 0)
                {
                    heroNode.Attributes.ModifyAttributeValue(AttributeType, BaseValueModifier);
                }

                if (MaxValueModifier != 0)
                {
                    heroNode.Attributes.ModifyMaxAttributeValue(AttributeType, MaxValueModifier);
                }
            }
            else
            {
                if (BaseValueModifier != 0)
                {
                    heroNode.Attributes.RegisterAttributeModifier(AttributeType, BaseValueModifier);
                }

                if (MaxValueModifier != 0)
                {
                    heroNode.Attributes.RegisterMaxAttributeModifier(AttributeType, MaxValueModifier);
                }
            }


            return Status.Complete;
        }
    }
}