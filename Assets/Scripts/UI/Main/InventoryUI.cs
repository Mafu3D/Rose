using System.Collections.Generic;
using Project.Items;
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

        [SerializeField] Inventory inventory;

        private void OnEnable()
        {
            inventory.OnInventoryChanged += UpdateUI;
        }

        private void OnDisable()
        {
            inventory.OnInventoryChanged -= UpdateUI;
        }

        // void Start()
        // {
        //     UpdateUI();
        // }

        private void UpdateUI()
        {
            Item equippedWeapon = inventory.GetEquippedWeapon();
            if (equippedWeapon != null)
            {
                weaponSlot.sprite = inventory.GetEquippedWeapon().ItemData.Sprite;
            }
            else
            {
                weaponSlot.sprite = unequippedSprite;
            }

            // offHandSlot.sprite = inventory.GetEquippedWeapon().ItemData.Sprite;
            offHandSlot.sprite = unequippedSprite;

            List<Item> heldItems = inventory.GetHeldItems();
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