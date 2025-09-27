namespace Project.Decks
{
    using System.Collections.Generic;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewDeckData", menuName = "Cards/Deck", order = 0)]
    public class DeckData : SerializedScriptableObject
    {
        [DictionaryDrawerSettings(KeyLabel = "Card", ValueLabel = "Amount")]
        public Dictionary<CardData, int> Cards = new Dictionary<CardData, int>();

        public List<Card> UnpackCards()
        {
            List<Card> unpackedCards = new();
            foreach (KeyValuePair<CardData, int> entry in Cards)
            {
                for (int i = 0; i < entry.Value; i++)
                {
                    CardData cd = entry.Key;
                    Card card = new Card(cd.Name, entry.Key.Text);
                    unpackedCards.Add(card);
                }
            }
            return unpackedCards;
        }
    }
}