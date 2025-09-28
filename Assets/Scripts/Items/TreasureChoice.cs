using System.Collections.Generic;

namespace Project.Items
{
    public class TreasureChoice
    {
        List<ItemData> itemDatas;
        public bool ItemHasBeenChosen = false;
        public TreasureChoice(List<ItemData> itemDatas)
        {
            this.itemDatas = itemDatas;
        }

        public void ChooseItem(int index)
        {
            if (!ItemHasBeenChosen)
            {
                GameManager.Instance.Player.Inventory.AddItem(itemDatas[index]);
                ItemHasBeenChosen = true;
            }
        }

        public ItemData GetItemData(int index)
        {
            return itemDatas[index];
        }
    }
}