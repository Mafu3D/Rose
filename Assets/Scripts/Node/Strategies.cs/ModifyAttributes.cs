using System.Collections.Generic;
using Project.Attributes;
using Project.GameNode.Hero;
using Unity.VisualScripting;
using UnityEngine;

namespace Project.GameNode.Strategies
{
    [CreateAssetMenu(fileName = "NewModifyAttributes", menuName = "Nodes/Modify Attributes", order = 1)]
    public class ModifyAttributes : ScriptableObject, INodeStrategy
    {
        [SerializeField] int HealthModifier;
        [SerializeField] int ArmorModifier;
        [SerializeField] int StrengthModifier;
        [SerializeField] int MagicModifier;
        [SerializeField] int DexterityModifier;
        [SerializeField] int SpeedModifier;

        public void Reset() { }

        public Status Resolve(Node other)
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