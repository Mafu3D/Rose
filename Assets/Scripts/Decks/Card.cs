using System.Collections.Generic;
using System.Linq;
using Project.GameTiles;
using Project.GameplayEffects;
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
        public readonly List<GameplayEffectStrategy> strategies;

        public Card(CardData data)
        {
            this.Name = data.Name;
            this.DisplayText = data.DisplayText;
            this.CardType = data.CardType;
            this.Sprite = data.Sprite;
            this.strategies = data.Strategies;
        }

        // public Status Resolve()
        // {
        //     return Status.Complete;
        // }

        public void Execute()
        {
            foreach (GameplayEffectStrategy effect in strategies)
            {
                GameManager.Instance.EffectQueue.QueueEffect(effect, null, null);
            }
        }
    }
}