namespace Project.Decks
{
    using System.Collections.Generic;
    using Project.GameTiles;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewTileDeckData", menuName = "Tiles/Tile Deck", order = 0)]
    public class TileDeckData : SerializedScriptableObject
    {
        [DictionaryDrawerSettings(KeyLabel = "Tile", ValueLabel = "Amount")]
        public Dictionary<TileData, int> Tiles = new Dictionary<TileData, int>();

        public List<TileData> UnpackItems()
        {
            List<TileData> unpackedItems = new();
            foreach (KeyValuePair<TileData, int> entry in Tiles)
            {
                for (int i = 0; i < entry.Value; i++)
                {
                    unpackedItems.Add(entry.Key);
                }
            }
            return unpackedItems;
        }
    }
}