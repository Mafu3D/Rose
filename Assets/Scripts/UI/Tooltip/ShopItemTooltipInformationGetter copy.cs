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
                content = itemData.Description;
                return true;
            }
            content = default;
            header = default;
            return false;
        }
    }
}
