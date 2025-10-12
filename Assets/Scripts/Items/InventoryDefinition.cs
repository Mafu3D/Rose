using System.Collections.Generic;
using UnityEngine;

namespace Project.Items
{
    [CreateAssetMenu(fileName = "NewInventoryDefinition", menuName = "Inventory Definition", order = 0)]
    public class InventoryDefinition : ScriptableObject
    {
        [SerializeField] public int MaxSlots = 4;
        [field: SerializeField] public ItemData StartingEquippedWeaponData = null;
        [field: SerializeField] public List<ItemData> StartingHeldItemsData = new();
    }
}