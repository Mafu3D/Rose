using System.Runtime.InteropServices.WindowsRuntime;
using Project.GameNode;

namespace Project.Items
{
    public enum ItemType
    {
        Basic,
        Weapon,
        Consumable
    }

    public class Item
    {
        public ItemData ItemData;
        Node ownerNode;
        public Item(ItemData itemData)
        {
            this.ItemData = itemData;

        }

        public void RegisterToNode(Node ownerNode)
        {
            this.ownerNode = ownerNode;
        }

        public void DeregisterFromNode()
        {
            this.ownerNode = null;
        }

        public virtual void OnEquip()
        {
            RegisterAttributeModifiers();
        }

        public virtual void OnUnequip()
        {
            DeregisterAttributeModifiers();
        }

        private void RegisterAttributeModifiers()
        {
            if (this.ItemData.HealthModifier != 0) ownerNode.Attributes.RegisterAttributeModifier(Attributes.AttributeType.Health, this.ItemData.HealthModifier);
            if (this.ItemData.ArmorModifier != 0) ownerNode.Attributes.RegisterAttributeModifier(Attributes.AttributeType.Armor, this.ItemData.ArmorModifier);
            if (this.ItemData.StrengthModifier != 0) ownerNode.Attributes.RegisterAttributeModifier(Attributes.AttributeType.Strength, this.ItemData.StrengthModifier);
            if (this.ItemData.MagicModifier != 0) ownerNode.Attributes.RegisterAttributeModifier(Attributes.AttributeType.Magic, this.ItemData.MagicModifier);
            if (this.ItemData.DexterityModifier != 0) ownerNode.Attributes.RegisterAttributeModifier(Attributes.AttributeType.Dexterity, this.ItemData.DexterityModifier);
            if (this.ItemData.SpeedModifier != 0) ownerNode.Attributes.RegisterAttributeModifier(Attributes.AttributeType.Speed, this.ItemData.SpeedModifier);
        }

        private void DeregisterAttributeModifiers()
        {
            if (this.ItemData.HealthModifier != 0) ownerNode.Attributes.DeregisterAttributeModifier(Attributes.AttributeType.Health, this.ItemData.HealthModifier);
            if (this.ItemData.ArmorModifier != 0) ownerNode.Attributes.DeregisterAttributeModifier(Attributes.AttributeType.Armor, this.ItemData.ArmorModifier);
            if (this.ItemData.StrengthModifier != 0) ownerNode.Attributes.DeregisterAttributeModifier(Attributes.AttributeType.Strength, this.ItemData.StrengthModifier);
            if (this.ItemData.MagicModifier != 0) ownerNode.Attributes.DeregisterAttributeModifier(Attributes.AttributeType.Magic, this.ItemData.MagicModifier);
            if (this.ItemData.DexterityModifier != 0) ownerNode.Attributes.DeregisterAttributeModifier(Attributes.AttributeType.Dexterity, this.ItemData.DexterityModifier);
            if (this.ItemData.SpeedModifier != 0) ownerNode.Attributes.DeregisterAttributeModifier(Attributes.AttributeType.Speed, this.ItemData.SpeedModifier);
        }

    }

    public class Weapon : Item
    {
        public Weapon(ItemData itemData) : base(itemData)
        {
        }
    }

    public class Consumable : Item
    {
        public Consumable(ItemData itemData) : base(itemData)
        {
        }
    }
}
