using Project.Items;
using Project.UI.Shop;
using UnityEngine;

namespace Project.UI
{
    public class ShopItemTooltipInformationGetter : TooltipInformationGetter
    {
        [SerializeField] ShopItemUI shopItemUI;

        public override bool TryGetTooltipInformation(out string content, out string header)
        {
            ItemData itemData = shopItemUI.GetItemData();
            if (itemData != null)
            {
                header = itemData.Name;

                content = "";
                switch (itemData.ItemType)
                {
                    case ItemType.Basic:
                        content += "Held";
                        break;
                    case ItemType.Weapon:
                        content += "Weapon";
                        break;
                    case ItemType.Offhand:
                        content += "Offhand";
                        break;
                }

                content += $"\n {itemData.Description}";

                return true;
            }
            content = default;
            header = default;
            return false;
        }
    }
}
