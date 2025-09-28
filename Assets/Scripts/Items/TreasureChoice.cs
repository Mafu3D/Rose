using System.Collections.Generic;

namespace Project.Items
{
    public class TreasureChoice
    {
        List<Item> items;
        public bool ItemHasBeenChosen = false;
        public TreasureChoice(List<Item> items)
        {
            this.items = items;
        }

        public void ChooseItem(int index)
        {
            if (!ItemHasBeenChosen)
            {
                Item item = items[index];
                switch (item.ItemData.ItemType)
                {
                    case ItemType.Weapon:
                        GameManager.Instance.Player.Inventory.SwapEquippedWeapon(item);
                        break;
                    default:
                        GameManager.Instance.Player.Inventory.AddItem(item);
                        break;
                }
                ItemHasBeenChosen = true;
            }
        }

        public Item GetItem(int index)
        {
            return items[index];
        }
    }
}