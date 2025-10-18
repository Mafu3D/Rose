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

        public List<ItemData> UnpackItems()
        {
            List<ItemData> unpackedItems = new();
            foreach (KeyValuePair<ItemData, int> entry in Cards)
            {
                int amount = 0;
                switch (entry.Key.Rarity)
                {
                    case ItemRarity.Common:
                        amount = 4;
                        break;
                    case ItemRarity.Uncommon:
                        amount = 3;
                        break;
                    case ItemRarity.Rare:
                        amount = 2;
                        break;
                    case ItemRarity.Legendary:
                        amount = 1;
                        break;
                }

                for (int i = 0; i < amount; i++)
                {
                    // ItemData data = entry.Key;
                    // Item card = new Item(data);
                    // unpackedItems.Add(card);
                    unpackedItems.Add(entry.Key);
                }
        }
            return unpackedItems;
        }
}
}