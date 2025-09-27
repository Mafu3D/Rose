using System.Collections.Generic;
using System.Linq;
using Project.GameNode;
using Projejct.Decks.CardStrategies;
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
        public readonly string Name;
        public readonly string DisplayText;
        public readonly Sprite Sprite;
        public readonly CardType CardType;
        public readonly int Value;
        public readonly List<IStrategy> strategies;

        public Card(CardData data)
        {
            this.Name = data.Name;
            this.DisplayText = data.DisplayText;
            this.CardType = data.CardType;
            this.Sprite = data.Sprite;
            this.Value = data.Value;
            this.strategies = data.Strategies;
        }

        public void Execute()
        {
            // Debug.Log($"{Name}: {DisplayText}");
            foreach (IStrategy strategy in strategies)
            {
                strategy.Execute();
            }
        }
    }
}