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
                GameManager.Instance.Player.Inventory.AddItem(items[index]);
                ItemHasBeenChosen = true;
            }
        }

        public Item GetItem(int index)
        {
            return items[index];
        }
    }
}