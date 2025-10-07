using System.Runtime.InteropServices.WindowsRuntime;
using Project.GameTiles;

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
        Tile owner;
        public Item(ItemData itemData)
        {
            this.ItemData = itemData;

        }

        public void RegisterToNode(Tile owner)
        {
            this.owner = owner;
        }

        public void DeregisterFromNode()
        {
            this.owner = null;
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
            if (this.ItemData.MaxHealthModifier != 0) owner.Character.Attributes.ModifyMaxAttributeValue(Attributes.AttributeType.Health, this.ItemData.MaxHealthModifier);
            if (this.ItemData.MaxArmorModifier != 0) owner.Character.Attributes.ModifyMaxAttributeValue(Attributes.AttributeType.Armor, this.ItemData.MaxArmorModifier);

            if (this.ItemData.MaxStrengthModifier != 0) owner.Character.Attributes.RegisterMaxAttributeModifier(Attributes.AttributeType.Strength, this.ItemData.MaxStrengthModifier);
            if (this.ItemData.MaxMagicModifier != 0) owner.Character.Attributes.RegisterMaxAttributeModifier(Attributes.AttributeType.Magic, this.ItemData.MaxMagicModifier);
            if (this.ItemData.MaxDexterityModifier != 0) owner.Character.Attributes.RegisterMaxAttributeModifier(Attributes.AttributeType.Dexterity, this.ItemData.MaxDexterityModifier);
            if (this.ItemData.MaxSpeedModifier != 0) owner.Character.Attributes.RegisterMaxAttributeModifier(Attributes.AttributeType.Speed, this.ItemData.MaxSpeedModifier);

            if (this.ItemData.HealthModifier != 0) owner.Character.Attributes.ModifyAttributeValue(Attributes.AttributeType.Health, this.ItemData.HealthModifier);
            if (this.ItemData.ArmorModifier != 0) owner.Character.Attributes.ModifyAttributeValue(Attributes.AttributeType.Armor, this.ItemData.ArmorModifier);

            if (this.ItemData.StrengthModifier != 0) owner.Character.Attributes.RegisterAttributeModifier(Attributes.AttributeType.Strength, this.ItemData.StrengthModifier);
            if (this.ItemData.MagicModifier != 0) owner.Character.Attributes.RegisterAttributeModifier(Attributes.AttributeType.Magic, this.ItemData.MagicModifier);
            if (this.ItemData.DexterityModifier != 0) owner.Character.Attributes.RegisterAttributeModifier(Attributes.AttributeType.Dexterity, this.ItemData.DexterityModifier);
            if (this.ItemData.SpeedModifier != 0) owner.Character.Attributes.RegisterAttributeModifier(Attributes.AttributeType.Speed, this.ItemData.SpeedModifier);
        }

        private void DeregisterAttributeModifiers()
        {
            if (this.ItemData.MaxHealthModifier != 0) owner.Character.Attributes.ModifyMaxAttributeValue(Attributes.AttributeType.Health, -this.ItemData.MaxHealthModifier);
            if (this.ItemData.MaxArmorModifier != 0) owner.Character.Attributes.ModifyMaxAttributeValue(Attributes.AttributeType.Armor, -this.ItemData.MaxArmorModifier);

            if (this.ItemData.MaxStrengthModifier != 0) owner.Character.Attributes.DeregisterMaxAttributeModifier(Attributes.AttributeType.Strength, this.ItemData.MaxStrengthModifier);
            if (this.ItemData.MaxMagicModifier != 0) owner.Character.Attributes.DeregisterMaxAttributeModifier(Attributes.AttributeType.Magic, this.ItemData.MaxMagicModifier);
            if (this.ItemData.MaxDexterityModifier != 0) owner.Character.Attributes.DeregisterMaxAttributeModifier(Attributes.AttributeType.Dexterity, this.ItemData.MaxDexterityModifier);
            if (this.ItemData.MaxSpeedModifier != 0) owner.Character.Attributes.DeregisterMaxAttributeModifier(Attributes.AttributeType.Speed, this.ItemData.MaxSpeedModifier);

            if (this.ItemData.HealthModifier != 0) owner.Character.Attributes.ModifyAttributeValue(Attributes.AttributeType.Health, -this.ItemData.HealthModifier);
            if (this.ItemData.ArmorModifier != 0) owner.Character.Attributes.ModifyAttributeValue(Attributes.AttributeType.Armor, -this.ItemData.ArmorModifier);

            if (this.ItemData.StrengthModifier != 0) owner.Character.Attributes.DeregisterAttributeModifier(Attributes.AttributeType.Strength, this.ItemData.StrengthModifier);
            if (this.ItemData.MagicModifier != 0) owner.Character.Attributes.DeregisterAttributeModifier(Attributes.AttributeType.Magic, this.ItemData.MagicModifier);
            if (this.ItemData.DexterityModifier != 0) owner.Character.Attributes.DeregisterAttributeModifier(Attributes.AttributeType.Dexterity, this.ItemData.DexterityModifier);
            if (this.ItemData.SpeedModifier != 0) owner.Character.Attributes.DeregisterAttributeModifier(Attributes.AttributeType.Speed, this.ItemData.SpeedModifier);
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
