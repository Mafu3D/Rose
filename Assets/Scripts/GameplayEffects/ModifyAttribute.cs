using System.Collections.Generic;
using Project.Attributes;
using Project.GameTiles;
using UnityEngine;

namespace Project.GameplayEffects
{
    [CreateAssetMenu(fileName = "NewModifyAttribute", menuName = "Effects/Modify Attribute", order = 1)]
    public class ModifyAttribute : GameplayEffectStrategy
    {
        [SerializeField] AttributeType AttributeType;
        [SerializeField] int BaseValueModifier;
        [SerializeField] int MaxValueModifier;
        [SerializeField] int Duration;

        public override void ResetEffect() { }
        public override Status ResolveEffect() { return Status.Complete; }

        public override Status StartEffect()
        {
            Tile heroTile = GameManager.Instance.Player.HeroTile;
            if (AttributeType == AttributeType.Health || AttributeType == AttributeType.Armor)
            {
                if (MaxValueModifier != 0)
                {
                    heroTile.Character.Attributes.ModifyMaxAttributeValue(AttributeType, MaxValueModifier);
                }

                if (BaseValueModifier != 0)
                {
                    heroTile.Character.Attributes.ModifyAttributeValue(AttributeType, BaseValueModifier);
                }
            }
            else
            {
                if (MaxValueModifier != 0)
                {
                    heroTile.Character.Attributes.RegisterMaxAttributeModifier(AttributeType, MaxValueModifier, Duration);
                }

                if (BaseValueModifier != 0)
                {
                    heroTile.Character.Attributes.RegisterAttributeModifier(AttributeType, BaseValueModifier, Duration);
                }
            }

            return Status.Complete;
        }
    }
}