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
        private Item equippedOffhand;
        private List<Item> heldItems = new();
        private List<Item> secretItems = new();
        private Item weaponUpgrade;
        private int maxSlots = 4;
        const int TOTAL_INVENTORY_SLOTS = 8;

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
        public Item GetEquippedOffhand() => equippedOffhand;

        public Item GetItemAtSlot(int index)
        {
            if (index >= heldItems.Count)
            {
                return null;
            }
            return heldItems[index];
        }
        public List<Item> GetHeldItems() => heldItems;
        public List<Item> GetSecretItems() => secretItems;
        public Item GetWeaponUpgrade() => weaponUpgrade;

        public List<Item> GetAllItems(bool includeSecretItems = false)
        {
            List<Item> allItems = new List<Item>(GetHeldItems());
            if (equippedWeapon != null)
            {
                allItems.Add(equippedWeapon);
                if (weaponUpgrade != null)
                {
                    allItems.Add(weaponUpgrade);
                }
            }
            if (equippedOffhand != null)
            {
                allItems.Add(equippedOffhand);
            }
            if (includeSecretItems)
            {
                List<Item> secretItems = new List<Item>(GetSecretItems());
                allItems.AddRange(secretItems);
            }
            return allItems;
        }

        public List<Item> GetAllItemsWithNulls()
        {
            List<Item> allItems = new List<Item>(GetHeldItems());
            for (int i = 0; i < TOTAL_INVENTORY_SLOTS; i++)
            {
                if (i >= allItems.Count)
                {
                    allItems.Add(null);
                }
            }

            if (equippedWeapon != null)
            {
                allItems.Add(equippedWeapon);
                if (weaponUpgrade != null)
                {
                    allItems.Add(weaponUpgrade);
                }
            }
            else
            {
                allItems.Add(null);
            }
            if (equippedOffhand != null)
            {
                allItems.Add(equippedOffhand);
            }
            else
            {
                allItems.Add(null);
            }
            return allItems;
        }

        public Item SwapEquippedWeapon(Item item)
        {
            if (equippedWeapon != null)
            {
                equippedWeapon.OnUnequip(owner);
                RemoveWeaponUpgrade();
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
            if (index < heldItems.Count)
            {
                Item item;
                heldItems.Pop(index, out item);
                item.OnUnequip(owner);

                OnInventoryChanged?.Invoke();
            }
        }

        public void RemoveItem(Item item)
        {
            if (item == equippedWeapon)
            {
                RemoveWeapon();
            }

            if (item == equippedOffhand)
            {
                RemoveOffhand();
            }

            if (heldItems.Contains(item))
            {
                heldItems.Pop(heldItems.IndexOf(item), out item);
                item.OnUnequip(owner);
                OnInventoryChanged?.Invoke();
            }
        }

        public void RemoveWeapon()
        {
            if (equippedWeapon != null) {
                equippedWeapon.OnUnequip(owner);
                RemoveWeaponUpgrade();
                equippedWeapon = null;
                OnInventoryChanged?.Invoke();
            }
        }

        public void RemoveOffhand()
        {
            if (equippedOffhand != null)
            {
                equippedOffhand.OnUnequip(owner);
                equippedOffhand = null;
                OnInventoryChanged?.Invoke();
            }
        }

        public int AddSecretItem(Item item)
        {
            secretItems.Add(item);
            item.OnEquip(owner);
            return secretItems.Count - 1;
        }

        public void RemoveSecretItem(int index)
        {
            Item item;
            secretItems.Pop(index, out item);
            item.OnUnequip(owner);
        }

        public void AddWeaponUpgrade(Item item)
        {
            if (weaponUpgrade != null)
            {
                weaponUpgrade.OnUnequip(owner);
            }
            weaponUpgrade = item;
            weaponUpgrade.OnEquip(owner);
        }

        public void RemoveWeaponUpgrade()
        {
            if (weaponUpgrade != null)
            {
                weaponUpgrade.OnUnequip(owner);
                weaponUpgrade = null;
            }
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