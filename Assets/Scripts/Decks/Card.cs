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
        public readonly List<ICardStrategy> strategies;
        public readonly List<GameplayEffectStrategy> gestrategies;
        private int resolvingStrategyIndex;
        private bool resolveingEffectHasStarted = false;

        public Card(CardData data)
        {
            this.Name = data.Name;
            this.DisplayText = data.DisplayText;
            this.CardType = data.CardType;
            this.Sprite = data.Sprite;
            this.Value = data.Value;
            this.strategies = data.Strategies;
            this.gestrategies = data.GEStrategies;
        }

        public Status Resolve()
        {
            while (resolvingStrategyIndex < gestrategies.Count)
            {
                if (!resolveingEffectHasStarted)
                {
                    gestrategies[resolvingStrategyIndex].Start();
                    resolveingEffectHasStarted = true;
                }
                Status status = gestrategies[resolvingStrategyIndex].Resolve();
                if (status != Status.Complete)
                {
                    return status;
                }
                resolvingStrategyIndex++;
                gestrategies[resolvingStrategyIndex].Reset();
                resolveingEffectHasStarted = false;
            }
            return Status.Complete;
        }

        public void Execute()
        {
            // Debug.Log($"{Name}: {DisplayText}");
            foreach (ICardStrategy strategy in strategies)
            {
                strategy.Execute();
            }
        }
    }
}