using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Project.Combat;
using Project.GameTiles;
using UnityEngine;

namespace Project.Items
{
    public class Inventory
    {
        private Item equippedWeapon;
        private List<Item> heldItems = new();
        private int maxSlots = 4;

        public event Action OnInventoryChanged;
        Character owner;

        public Inventory(Character owner)
        {
            this.owner = owner;
        }

        public Inventory(Character owner, InventoryDefinition inventoryDefinition)
        {
            this.owner = owner;
            EquipStartingItems(inventoryDefinition);
            maxSlots = inventoryDefinition.MaxSlots;
        }

        public Item GetEquippedWeapon() => equippedWeapon;

        public Item GetItemAtSlot(int index)
        {
            if (index >= heldItems.Count)
            {
                return null;
            }
            return heldItems[index];
        }
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
                equippedWeapon.OnUnequip(owner);
            }

            equippedWeapon = item;

            item.OnEquip(owner);

            OnInventoryChanged?.Invoke();

            return equippedWeapon;
        }

        public int AddItem(Item item)
        {
            // Check for max slots
            if (heldItems.Count >= maxSlots)
            {
                Debug.LogWarning($"Cannot equip item {item.ItemData.Name}! Already at max slots! Fix me!");
            }

            heldItems.Add(item);
            item.OnEquip(owner);

            OnInventoryChanged?.Invoke();
            return heldItems.Count - 1;
        }

        public void RemoveItem(int index)
        {
            Item item;
            heldItems.Pop(index, out item);
            item.OnUnequip(owner);

            OnInventoryChanged?.Invoke();
        }

        private void EquipStartingItems(InventoryDefinition inventoryDefinition)
        {
            foreach (ItemData itemData in inventoryDefinition.StartingHeldItemsData)
            {
                Item item = new Item(itemData);
                AddItem(item);
            }
            if (inventoryDefinition.StartingEquippedWeaponData)
            {
                Item item = new Item(inventoryDefinition.StartingEquippedWeaponData);
                SwapEquippedWeapon(item);
            }
        }
    }
}