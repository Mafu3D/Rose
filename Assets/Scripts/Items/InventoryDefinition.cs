using System.Collections.Generic;
using UnityEngine;

namespace Project.Items
{
    [CreateAssetMenu(fileName = "NewInventoryDefinition", menuName = "Inventory Definition", order = 0)]
    public class InventoryDefinition : ScriptableObject
    {
        [SerializeField] public int MaxSlots = 4;
        [SerializeField] public int MaxConsumableSlots { get; internal set; }

        [field: SerializeField] public ItemData StartingEquippedWeaponData = null;
        [field: SerializeField] public ItemData StartingEquippedOffhandData = null;
        [field: SerializeField] public List<ItemData> StartingHeldItemsData = new();
        [field: SerializeField] public List<ItemData> StartingConsumablesItemData = new();

    }
}