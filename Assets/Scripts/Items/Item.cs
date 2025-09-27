namespace Project.Items
{
    public class Item
    {
        ItemData data;
        public Item(ItemData itemData)
        {
            this.data = itemData;
        }

        public void OnEquip()
        {
        }

        public void OnUnequip() { }

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