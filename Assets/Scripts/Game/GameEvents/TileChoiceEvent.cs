using System.Collections.Generic;
using System.Linq;
using Project.GameTiles;
using UnityEngine;

namespace Project.Core.GameEvents
{
    public class TileChoiceEvent : ChoiceEvent<TileData>
    {
        private bool shuffleRemaining;
        public TileChoiceEvent(int amount, bool shuffleRemaining) : base(amount)
        {
            this.shuffleRemaining = shuffleRemaining;
        }

        public override void GenerateChoices()
        {
            List<TileData> choices = GameManager.Instance.TileDeck.DrawMultiple(amount);
            if (choices.Count == 0) return;

            // Bad luck protection - redraw until there is at least one option that doesnt have a cost
            int i = 0;
            List<TileData> toShuffleBackIn = new();
            while (choices.All(obj => obj.Cost > 0))
            {
                i++;
                if (i > GameManager.Instance.TileDeck.CurrentCount)
                {
                    Debug.LogWarning("sorted through entire deck without protecting bad luck!");
                    break;
                }
                toShuffleBackIn.Add(choices[0]);
                choices[0] = GameManager.Instance.TileDeck.Draw();
            }
            GameManager.Instance.TileDeck.AddToRemaining(toShuffleBackIn);
            GameManager.Instance.TileDeck.Shuffle();

            Choice = new Choice<TileData>(choices, ResolveCallback);
        }

        public override void ChooseItem(int index)
        {
            Choice.ChooseItem(index);
        }

        protected override void ResolveCallback(List<TileData> chosen, List<TileData> notChosen)
        {
            foreach (TileData tileData in chosen)
            {
                TileFactory.Instance.CreateTile(tileData, GameManager.Instance.Hero.CurrentCell.Center);
            }

            if (shuffleRemaining)
            {
                GameManager.Instance.TileDeck.AddToRemaining(notChosen, true);
            }
        }

        public override void Resolve()
        {
            Choice.Resolve();
        }
    }
}