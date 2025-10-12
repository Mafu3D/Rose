namespace Project.Decks
{
    using System.Collections.Generic;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewCharacterDeckData", menuName = "Characters/Deck", order = 0)]
    public class CharacterDeckData : SerializedScriptableObject
    {
        [DictionaryDrawerSettings(KeyLabel = "Character", ValueLabel = "Amount")]
        public Dictionary<CharacterData, int> Cards = new Dictionary<CharacterData, int>();

        public List<CharacterData> Unpack()
        {
            List<CharacterData> unpackedChars = new();
            foreach (KeyValuePair<CharacterData, int> entry in Cards)
            {
                for (int i = 0; i < entry.Value; i++)
                {
                    unpackedChars.Add(entry.Key);
                }
            }
            return unpackedChars;
        }
    }
}