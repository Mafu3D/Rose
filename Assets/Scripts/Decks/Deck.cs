using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Decks
{
    public enum CardType
    {
        Location,
        Encounter,
        Event,
        NPC,
        Monster,
        Item
    }

    public class Card
    {
        public readonly string name;
        public readonly string text;

        public Card(string name, string text)
        {
            this.name = name;
            this.text = text;
        }
    }

    public class Deck
    {
        public List<Card> StartingCards = new();
        public List<Card> RemainingCards = new();
        public int NumberRemaining => RemainingCards.Count;

        public void AddCards(List<Card> cardsToAdd)
        {
            foreach (Card card in cardsToAdd)
            {
                StartingCards.Add(card);
                RemainingCards.Add(card);
            }
        }

        public void Shuffle()
        {
            RemainingCards.Shuffle();
        }

        public void Reset()
        {
            RemainingCards = new List<Card>(StartingCards);
            Shuffle();
        }

        public Card DrawCard(int index = 0)
        {
            if (RemainingCards.Count == 0 || index > RemainingCards.Count - 1)
            {
                return null;
            }

            Card drawnCard;
            RemainingCards.Pop(index, out drawnCard);
            return drawnCard;
        }

        public List<Card> DrawCards(int amount, int startIndex = 0)
        {
            List<Card> drawnCards = new();
            for (int i = 0; i < amount; i++)
            {
                Card card = DrawCard(startIndex); // Don't need to add i since the list is popped
                if (card != null)
                {
                    drawnCards.Add(card);
                }
            }
            return drawnCards;
        }
    }
}