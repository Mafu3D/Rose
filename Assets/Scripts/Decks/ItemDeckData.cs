namespace Project.Decks
{
    using System.Collections.Generic;
    using Project.Items;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewItemDeckData", menuName = "Cards/Item Deck", order = 0)]
    public class ItemDeckData : SerializedScriptableObject
    {
        [DictionaryDrawerSettings(KeyLabel = "Item", ValueLabel = "Amount")]
        public Dictionary<ItemData, int> Cards = new Dictionary<ItemData, int>();

        public List<Item> UnpackItems()
        {
            List<Item> unpackedItems = new();
            foreach (KeyValuePair<ItemData, int> entry in Cards)
            {
                for (int i = 0; i < entry.Value; i++)
                {
                    ItemData data = entry.Key;
                    Item card = new Item(data);
                    unpackedItems.Add(card);
                }
            }
            return unpackedItems;
        }
    }
}