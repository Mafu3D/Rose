using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Project.Combat;
using Project.GameTiles;
using UnityEngine;

namespace Project.Items
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private int maxSlots = 4;
        [field: SerializeField] public ItemData startingEquippedWeaponData = null;
        [field: SerializeField] public List<ItemData> startingHeldItemsData = new();

        private Item equippedWeapon;
        private List<Item> heldItems = new();

        Tile owner;

        public event Action OnInventoryChanged;

        void Awake()
        {
            owner = GetComponent<Tile>();
        }

        void Start()
        {
            EquipStartingItems();
        }

        public Item GetEquippedWeapon() => equippedWeapon;

        public Item GetItemAtSlot(int index) => heldItems[index];
        public List<Item> GetHeldItems() => heldItems;

        public List<Item> GetAllItems()
        {
            List<Item> allItems = new List<Item>(GetHeldItems());
            if (equippedWeapon != null)
            {
                allItems.Add(equippedWeapon);
            }
            return allItems;
        }

        public Item SwapEquippedWeapon(Item item)
        {
            if (equippedWeapon != null)
            {
                equippedWeapon.OnUnequip();
                equippedWeapon.DeregisterFromNode();
            }

            equippedWeapon = item;

            item.RegisterToNode(owner);
            item.OnEquip();

            OnInventoryChanged?.Invoke();

            return equippedWeapon;
        }

        public int AddItem(Item item)
        {
            // Check for max slots

            heldItems.Add(item);
            item.RegisterToNode(owner);
            item.OnEquip();

            OnInventoryChanged?.Invoke();
            return heldItems.Count - 1;
        }

        public void RemoveItem(int index)
        {
            Item item;
            heldItems.Pop(index, out item);
            item.OnUnequip();
            item.DeregisterFromNode();

            OnInventoryChanged?.Invoke();
        }

        private void EquipStartingItems()
        {
            foreach (ItemData itemData in startingHeldItemsData)
            {
                Item item = new Item(itemData);
                AddItem(item);
            }
            if (startingEquippedWeaponData)
            {
                Item item = new Item(startingEquippedWeaponData);
                SwapEquippedWeapon(item);
            }
        }
    }
}