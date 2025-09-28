using System.Collections.Generic;
using Project.Attributes;
using Project.GameNode;
using Project.GameNode.Hero;
using UnityEngine;

namespace Project.GameplayEffects
{
    [CreateAssetMenu(fileName = "NewModifyAttributes", menuName = "Effects/Modify Attributes", order = 1)]
    public class ModifyAttributes : GameplayEffectStrategy
    {
        [SerializeField] int HealthModifier;
        [SerializeField] int ArmorModifier;
        [SerializeField] int StrengthModifier;
        [SerializeField] int MagicModifier;
        [SerializeField] int DexterityModifier;
        [SerializeField] int SpeedModifier;

        public override void Reset() { }
        public override Status Resolve() { return Status.Complete; }

        public override Status Start()
        {
            HeroNode heroNode = GameManager.Instance.Player.HeroNode;

            if (HealthModifier != 0) heroNode.Attributes.RegisterAttributeModifier(AttributeType.Health, HealthModifier);
            if (ArmorModifier != 0) heroNode.Attributes.RegisterAttributeModifier(AttributeType.Armor, ArmorModifier);
            if (StrengthModifier != 0) heroNode.Attributes.RegisterAttributeModifier(AttributeType.Strength, StrengthModifier);
            if (MagicModifier != 0) heroNode.Attributes.RegisterAttributeModifier(AttributeType.Magic, MagicModifier);
            if (DexterityModifier != 0) heroNode.Attributes.RegisterAttributeModifier(AttributeType.Dexterity, DexterityModifier);
            if (SpeedModifier != 0) heroNode.Attributes.RegisterAttributeModifier(AttributeType.Speed, SpeedModifier);

            return Status.Complete;
        }
    }
}