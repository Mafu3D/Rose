using Project.Items;
using UnityEngine;

namespace Project.UI
{
    public class InventoryTooltipInformationGetter : TooltipInformationGetter
    {
        [SerializeField] int slot;
        [SerializeField] bool isWeapon = false;
        public override bool TryGetTooltipInformation(out string content, out string header)
        {
            Item item;
            if (isWeapon)
            {
                item = GameManager.Instance.Player.HeroTile.Character.Inventory.GetEquippedWeapon();
            }
            else
            {
                item = GameManager.Instance.Player.HeroTile.Character.Inventory.GetItemAtSlot(slot);
            }
            if (item != null)
            {
                header = item.ItemData.Name;
                content = item.ItemData.Description;
                return true;
            }
            content = default;
            header = default;
            return false;
        }
    }
}
