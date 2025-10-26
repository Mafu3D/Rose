using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Project.Combat;
using Project.GameplayEffects;
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
        private List<Item> consumableItems = new();
        private List<Item> activeConsumableItems = new();
        private Item weaponUpgrade;
        private int maxSlots = 4;
        private int maxConsumableSlots = 4;
        const int TOTAL_INVENTORY_SLOTS = 8;
        const int TOTAL_CONSUMABLE_SLOTS = 4;

        public event Action OnInventoryChanged;
        Character owner;

        public bool InventoryIsFull => heldItems.Count >= maxSlots;
        public bool ConsumablesAreFull => consumableItems.Count >= maxConsumableSlots;

        public Inventory(Character owner)
        {
            this.owner = owner;
        }

        public Inventory(Character owner, InventoryDefinition inventoryDefinition)
        {
            this.owner = owner;
            EquipStartingItems(inventoryDefinition);
            maxSlots = inventoryDefinition.MaxSlots;
            maxConsumableSlots = inventoryDefinition.MaxConsumableSlots;
        }

        public void LoadInventory(InventoryDefinition inventoryDefinition)
        {
            UnequipAllItems();
            EquipStartingItems(inventoryDefinition);
            maxSlots = inventoryDefinition.MaxSlots;
            maxConsumableSlots = inventoryDefinition.MaxConsumableSlots;
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
        public Item GetConsumableItemAtSlot(int index)
        {
            if (index >= consumableItems.Count) return null;
            return consumableItems[index];
        }
        public List<Item> GetHeldItems() => heldItems;
        public List<Item> GetConsumableItems() => consumableItems;
        public List<Item> GetActiveConsumableItems() => activeConsumableItems;
        public List<Item> GetSecretItems() => secretItems;
        public Item GetWeaponUpgrade() => weaponUpgrade;

        public List<Item> GetAllItems(bool includeSecretItems = false)
        {
            List<Item> allItems = new();

            List<Item> activeConsumables = new List<Item>(GetActiveConsumableItems());
            allItems.AddRange(activeConsumables);

            List<Item> heldItems = new List<Item>(GetHeldItems());
            allItems.AddRange(heldItems);

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

        public Item SwapEquippedOffhand(Item item)
        {
            if (equippedOffhand != null)
            {
                equippedOffhand.OnUnequip(owner);
                RemoveWeaponUpgrade();
            }

            equippedOffhand = item;

            item.OnEquip(owner);

            OnInventoryChanged?.Invoke();

            return equippedOffhand;
        }

        public int AddItem(Item item)
        {
            // Check for max slots
            if (heldItems.Count >= maxSlots)
            {
                Debug.LogWarning($"Cannot equip item {item.ItemData.Name}! Already at max slots! Fix me!");
            }

            switch (item.ItemData.ItemType)
            {
                case ItemType.Basic:
                    heldItems.Add(item);
                    item.OnEquip(owner);
                    break;
                case ItemType.Weapon:
                    SwapEquippedWeapon(item);
                    break;
                case ItemType.Offhand:
                    SwapEquippedOffhand(item);
                    break;
                case ItemType.Consumable:
                    consumableItems.Add(item);
                    item.OnEquip(owner);
                    break;
            }

            OnInventoryChanged?.Invoke();
            return heldItems.Count - 1;
        }

        public void RemoveItem(int index)
        {
            // only goes by index of held items
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

            if (consumableItems.Contains(item))
            {
                consumableItems.Pop(consumableItems.IndexOf(item), out item);
                item.OnUnequip(owner);
                OnInventoryChanged?.Invoke();
            }
            else if (heldItems.Contains(item))
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

        public void MoveConsumableItem(int index)
        {
            if (index < consumableItems.Count)
            {
                Item item;
                consumableItems.Pop(index, out item);
                activeConsumableItems.Add(item);
                OnInventoryChanged?.Invoke();
            }
        }

        public void ClearActiveConsumableItems()
        {
            activeConsumableItems.Clear();
            OnInventoryChanged?.Invoke();
        }

        private void EquipStartingItems(InventoryDefinition inventoryDefinition)
        {
            foreach (ItemData itemData in inventoryDefinition.StartingHeldItemsData)
            {
                Item item = new Item(itemData);
                AddItem(item);
            }
            foreach (ItemData itemData in inventoryDefinition.StartingConsumablesItemData)
            {
                Item item = new Item(itemData);
                AddItem(item);
            }
            if (inventoryDefinition.StartingEquippedWeaponData)
            {
                Item item = new Item(inventoryDefinition.StartingEquippedWeaponData);
                SwapEquippedWeapon(item);
            }
            if (inventoryDefinition.StartingEquippedOffhandData)
            {
                Item item = new Item(inventoryDefinition.StartingEquippedOffhandData);
                SwapEquippedOffhand(item);
            }
        }

        private void UnequipAllItems()
        {
            foreach (Item item in heldItems)
            {
                RemoveItem(item);
            }
            RemoveWeapon();
            RemoveOffhand();
        }

        internal bool TryUseConsumableItemOverworld(int index)
        {
            Item item = consumableItems[index];
            if (item.ItemData.OnOverworldUse.Count == 0) return false;
            foreach (GameplayEffectStrategy effect in item.ItemData.OnOverworldUse)
            {
                GameManager.Instance.EffectQueue.AddEffect(effect);
            }
            GameManager.Instance.EffectQueue.ResolveQueue();
            MoveConsumableItem(index);
            return true;
        }

        internal void UseConsumableItemPrecombat(int index)
        {
            Debug.Log($"usin {index}");
            foreach(Item item in consumableItems)
            {
                Debug.Log(item.ItemData.Name);
            }
            MoveConsumableItem(index);
        }
    }
}