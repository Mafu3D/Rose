using Project.Items;
using UnityEngine;

namespace Project.UI
{
    public class InventoryTooltipInformationGetter : TooltipInformationGetter
    {
        [SerializeField] int slot;
        [SerializeField] bool isWeapon = false;
        [SerializeField] bool isOffhand = false;
        [SerializeField] bool isConsumable = false;
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
            else if (isConsumable)
            {
                item = GameManager.Instance.Player.HeroTile.Character.Inventory.GetConsumableItemAtSlot(slot);
            }
            else
            {
                item = GameManager.Instance.Player.HeroTile.Character.Inventory.GetItemAtSlot(slot);
            }
            if (item != null)
            {
                header = item.ItemData.Name;
                content = "";
                switch (item.ItemData.ItemType)
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
                    case ItemType.Consumable:
                        content += "Consumable";
                        break;
                }

                content += $"\n {item.ItemData.Description}";
                content += "\n \n WARNING: Clicking this will DISCARD the item FOREVER!";
                return true;
            }
            content = default;
            header = default;
            return false;
        }
    }
}
