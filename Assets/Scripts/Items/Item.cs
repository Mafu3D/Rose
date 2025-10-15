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
        public int Uses { get; private set; }
        public Item(ItemData itemData)
        {
            this.ItemData = itemData;
        }

        public virtual void OnEquip(Character character)
        {
            RegisterAttributeModifiers(character);
        }

        public virtual void OnUnequip(Character character)
        {
            DeregisterAttributeModifiers(character);
        }

        private void RegisterAttributeModifiers(Character character)
        {
            if (this.ItemData.MaxHealthModifier != 0) character.Attributes.ModifyMaxAttributeValue(Attributes.AttributeType.Health, this.ItemData.MaxHealthModifier);
            if (this.ItemData.MaxArmorModifier != 0) character.Attributes.ModifyMaxAttributeValue(Attributes.AttributeType.Armor, this.ItemData.MaxArmorModifier);

            if (this.ItemData.MaxStrengthModifier != 0) character.Attributes.RegisterMaxAttributeModifier(Attributes.AttributeType.Strength, this.ItemData.MaxStrengthModifier);
            if (this.ItemData.MaxMagicModifier != 0) character.Attributes.RegisterMaxAttributeModifier(Attributes.AttributeType.Magic, this.ItemData.MaxMagicModifier);
            if (this.ItemData.MaxDexterityModifier != 0) character.Attributes.RegisterMaxAttributeModifier(Attributes.AttributeType.Dexterity, this.ItemData.MaxDexterityModifier);
            if (this.ItemData.MaxSpeedModifier != 0) character.Attributes.RegisterMaxAttributeModifier(Attributes.AttributeType.Speed, this.ItemData.MaxSpeedModifier);

            if (this.ItemData.HealthModifier != 0) character.Attributes.ModifyAttributeValue(Attributes.AttributeType.Health, this.ItemData.HealthModifier);
            if (this.ItemData.ArmorModifier != 0) character.Attributes.ModifyAttributeValue(Attributes.AttributeType.Armor, this.ItemData.ArmorModifier);

            if (this.ItemData.StrengthModifier != 0) character.Attributes.RegisterAttributeModifier(Attributes.AttributeType.Strength, this.ItemData.StrengthModifier);
            if (this.ItemData.MagicModifier != 0) character.Attributes.RegisterAttributeModifier(Attributes.AttributeType.Magic, this.ItemData.MagicModifier);
            if (this.ItemData.DexterityModifier != 0) character.Attributes.RegisterAttributeModifier(Attributes.AttributeType.Dexterity, this.ItemData.DexterityModifier);
            if (this.ItemData.SpeedModifier != 0) character.Attributes.RegisterAttributeModifier(Attributes.AttributeType.Speed, this.ItemData.SpeedModifier);

            if (this.ItemData.MultistrikeModifier != 0) character.Attributes.RegisterAttributeModifier(Attributes.AttributeType.Multistrike, this.ItemData.MultistrikeModifier);
        }

        private void DeregisterAttributeModifiers(Character character)
        {
            if (this.ItemData.MaxHealthModifier != 0) character.Attributes.ModifyMaxAttributeValue(Attributes.AttributeType.Health, -this.ItemData.MaxHealthModifier);
            if (this.ItemData.MaxArmorModifier != 0) character.Attributes.ModifyMaxAttributeValue(Attributes.AttributeType.Armor, -this.ItemData.MaxArmorModifier);

            if (this.ItemData.MaxStrengthModifier != 0) character.Attributes.DeregisterMaxAttributeModifier(Attributes.AttributeType.Strength, this.ItemData.MaxStrengthModifier);
            if (this.ItemData.MaxMagicModifier != 0) character.Attributes.DeregisterMaxAttributeModifier(Attributes.AttributeType.Magic, this.ItemData.MaxMagicModifier);
            if (this.ItemData.MaxDexterityModifier != 0) character.Attributes.DeregisterMaxAttributeModifier(Attributes.AttributeType.Dexterity, this.ItemData.MaxDexterityModifier);
            if (this.ItemData.MaxSpeedModifier != 0) character.Attributes.DeregisterMaxAttributeModifier(Attributes.AttributeType.Speed, this.ItemData.MaxSpeedModifier);

            if (this.ItemData.HealthModifier != 0) character.Attributes.ModifyAttributeValue(Attributes.AttributeType.Health, -this.ItemData.HealthModifier);
            if (this.ItemData.ArmorModifier != 0) character.Attributes.ModifyAttributeValue(Attributes.AttributeType.Armor, -this.ItemData.ArmorModifier);

            if (this.ItemData.StrengthModifier != 0) character.Attributes.DeregisterAttributeModifier(Attributes.AttributeType.Strength, this.ItemData.StrengthModifier);
            if (this.ItemData.MagicModifier != 0) character.Attributes.DeregisterAttributeModifier(Attributes.AttributeType.Magic, this.ItemData.MagicModifier);
            if (this.ItemData.DexterityModifier != 0) character.Attributes.DeregisterAttributeModifier(Attributes.AttributeType.Dexterity, this.ItemData.DexterityModifier);
            if (this.ItemData.SpeedModifier != 0) character.Attributes.DeregisterAttributeModifier(Attributes.AttributeType.Speed, this.ItemData.SpeedModifier);

            if (this.ItemData.MultistrikeModifier != 0) character.Attributes.DeregisterAttributeModifier(Attributes.AttributeType.Multistrike, this.ItemData.MultistrikeModifier);
        }

        public void IncUses()
        {
            Uses += 1;
        }

        public void ResetUses()
        {
            Uses = 0;
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
