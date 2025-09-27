using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace Project.Items
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private int maxSlots;
        [field: SerializeField] public ItemData startingEquippedWeaponData = null;
        [field: SerializeField] public List<ItemData> startingHeldItemsData = new();

        private Item equippedWeapon;
        private List<Item> heldItems = new();


        public Item GetEquippedWeapon() => equippedWeapon;

        public Item GetItemAtSlot(int index) => heldItems[index];

        public Item SwapEquippedWeapon(ItemData itemData)
        {
            Item item = new Item(itemData);
            equippedWeapon = item;
            return equippedWeapon;
        }

        public int AddItem(ItemData itemData)
        {
            Item item = new Item(itemData);
            heldItems.Add(item);
            return heldItems.Count - 1;
        }

        public void RemoveItem(int index)
        {
            heldItems.RemoveAt(index);
        }
    }
}