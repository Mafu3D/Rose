using System.Collections.Generic;
using Project.Items;
using Project.PlayerSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.MainUI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private Image weaponSlot;
        [SerializeField] private Image offHandSlot;
        [SerializeField] private List<Image> heldItemSlots = new ();
        [SerializeField] private Sprite unequippedSprite;

        [SerializeField] Player player;

        private void OnEnable()
        {
            // player.HeroTile.Character.Inventory.OnInventoryChanged += UpdateUI;
        }

        private void OnDisable()
        {
            player.HeroTile.Character.Inventory.OnInventoryChanged -= UpdateUI;
        }

        void Start()
        {
            player.HeroTile.Character.Inventory.OnInventoryChanged += UpdateUI;
            UpdateUI();
        }

        private void UpdateUI()
        {
            Item equippedWeapon = player.HeroTile.Character.Inventory.GetEquippedWeapon();
            if (equippedWeapon != null)
            {
                weaponSlot.sprite = player.HeroTile.Character.Inventory.GetEquippedWeapon().ItemData.Sprite;
            }
            else
            {
                weaponSlot.sprite = unequippedSprite;
            }

            Item equippedOffhand = player.HeroTile.Character.Inventory.GetEquippedOffhand();
            if (equippedOffhand != null)
            {
                offHandSlot.sprite = player.HeroTile.Character.Inventory.GetEquippedOffhand().ItemData.Sprite;
            }
            else
            {
                offHandSlot.sprite = unequippedSprite;
            }

            // offHandSlot.sprite = player.HeroTile.Character.Inventory.GetEquippedWeapon().ItemData.Sprite;
            // offHandSlot.sprite = unequippedSprite;

            List<Item> heldItems = player.HeroTile.Character.Inventory.GetHeldItems();
            for (int i = 0; i < heldItemSlots.Count; i++)
            {
                if (heldItems.Count > i)
                {
                    heldItemSlots[i].sprite = heldItems[i].ItemData.Sprite;
                }
                else
                {
                    heldItemSlots[i].sprite = unequippedSprite;
                }
            }
        }
    }
}