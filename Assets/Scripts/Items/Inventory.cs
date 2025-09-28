using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Project.GameNode;
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

        CombatNode ownerNode;

        public event Action OnInventoryChanged;

        void Awake()
        {
            ownerNode = GetComponent<CombatNode>();
        }

        void Start()
        {
            EquipStartingItems();
        }

        public Item GetEquippedWeapon() => equippedWeapon;

        public Item GetItemAtSlot(int index) => heldItems[index];
        public List<Item> GetHeldItems() => heldItems;

        public Item SwapEquippedWeapon(Item item)
        {
            if (equippedWeapon != null)
            {
                equippedWeapon.OnEquip();
                equippedWeapon.DeregisterFromNode();
            }

            equippedWeapon = item;

            item.RegisterToNode(ownerNode);
            item.OnEquip();

            OnInventoryChanged?.Invoke();

            return equippedWeapon;
        }

        public int AddItem(Item item)
        {
            // Check for max slots

            heldItems.Add(item);
            item.RegisterToNode(ownerNode);
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