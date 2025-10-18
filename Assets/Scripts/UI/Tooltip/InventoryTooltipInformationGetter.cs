using Project.Items;
using UnityEngine;

namespace Project.UI
{
    public class InventoryTooltipInformationGetter : TooltipInformationGetter
    {
        [SerializeField] int slot;
        [SerializeField] bool isWeapon = false;
        [SerializeField] bool isOffhand = false;
        public override bool TryGetTooltipInformation(out string content, out string header)
        {
            Item item;
            if (isWeapon)
            {
                item = GameManager.Instance.Player.HeroTile.Character.Inventory.GetEquippedWeapon();
            }
            else if (isOffhand)
            {
                item = GameManager.Instance.Player.HeroTile.Character.Inventory.GetEquippedOffhand();
            }
            else
            {
                item = GameManager.Instance.Player.HeroTile.Character.Inventory.GetItemAtSlot(slot);
            }
            if (item != null)
            {
                header = item.ItemData.Name;
                content = item.ItemData.Description;
                content += "\n \n WARNING: Clicking this will DISCARD the item FOREVER!";
                return true;
            }
            content = default;
            header = default;
            return false;
        }
    }
}
