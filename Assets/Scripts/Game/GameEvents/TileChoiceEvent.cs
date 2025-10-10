using System.Collections.Generic;
using Project.GameTiles;

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